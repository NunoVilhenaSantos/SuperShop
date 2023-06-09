using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SuperShop.Web.Utils.ConfigOptions;

namespace SuperShop.Web.Services
{
    public class CloudStorageService : ICloudStorageService
    {
        private readonly GoogleCredential _googleCredentials;
        private readonly ILogger<CloudStorageService> _logger;


        //private readonly IOptions<GCPConfigOptions> _options;
        private readonly GCPConfigOptions _options;


        public CloudStorageService(
            IOptions<GCPConfigOptions> options,
            ILogger<CloudStorageService> logger)
        {
            _options = options.Value;
            _logger = logger;


            //_googleCredentials = GoogleCredential.FromFile(_options.CredentialsFilePath).CreateScoped(scopes: _options.Scopes).UnderlyingCredential
            //as Google.Apis.Auth.OAuth2.GoogleCredential;


            try
            {
                _googleCredentials =
                    GoogleCredential.FromFile(_options.GCPStorageAuthFile_Nuno);

                var environment = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                if (environment == "Development")
                {
                    _googleCredentials = GoogleCredential.FromFile(_options.GCPStorageAuthFile_Nuno);
                }
                else
                {
                    _googleCredentials = GoogleCredential.GetApplicationDefault();

                    // store the json file in secrets.
                    _googleCredentials = GoogleCredential.FromJson(_options.GCPStorageAuthFile_Nuno);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex.Message}");
            }
        }


        public async Task<bool> DeleteFileAsync(string fileNameToDelete)
        {
            try
            {
                _logger.LogInformation(
                    $"Deleting File Async: {fileNameToDelete} into storage {_options.GCPStorageBucketName_Nuno}");


                using (var storageClient =
                       StorageClient.Create(_googleCredentials))
                {
                    await storageClient.DeleteObjectAsync(
                        _options.GCPStorageBucketName_Nuno, fileNameToDelete);

                    _logger.LogInformation(
                        $"Deleted File Async: {fileNameToDelete} into storage {_options.GCPStorageBucketName_Nuno}");

                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex.Message}");
                return false;
                // return await Task.FromResult($"Error while deleting file {fileNameToDelete} {ex.Message}").ConfigureAwait(false);
            }
        }

        [Obsolete]
        public async Task<string> GetSignedUrlAsync(string fileNameToRead,
            int timeOutInMinutes = 2)
        {
            try
            {
                _logger.LogInformation(
                    $"Obtained signed url for the file {fileNameToRead} in the storage {_options.GCPStorageBucketName_Nuno}");

                var urlSigner =
                    UrlSigner.FromServiceAccountPath(_options
                        .GCPStorageAuthFile_Nuno);

                // V4 is the default signing version.
                var signedUrl = await urlSigner.SignAsync(
                    _options.GCPStorageBucketName_Nuno, fileNameToRead,
                    TimeSpan.FromHours(1), HttpMethod.Get);

                Console.WriteLine("Generated GET signed URL:");
                Console.WriteLine(signedUrl);
                Console.WriteLine(
                    "You can use this URL with any user agent, for example:");
                Console.WriteLine($"curl '{signedUrl}'");


                _logger.LogInformation(
                    $"Obtained signed url for the file {fileNameToRead} in the storage {_options.GCPStorageBucketName_Nuno}");

                return signedUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex.Message}");
                return await Task.FromResult(
                    $"Error while obtaining signed url for the file {fileNameToRead} {ex.Message}");
            }
        }


        public async Task<string> UploadFileAsync(IFormFile fileToUpload,
            string fileNameToSave)
        {
            try
            {
                _logger.LogInformation(
                    $"Uploading File Async: {fileToUpload.FileName} to {fileNameToSave} into storage {_options.GCPStorageBucketName_Nuno}");


                using (var memoryStream = new MemoryStream())
                {
                    await fileToUpload.CopyToAsync(memoryStream);

                    // create storage client using the credentials file.
                    using (var storageClient =
                           StorageClient.Create(_googleCredentials))
                    {
                        //var bucketName = _options.GCPStorageBucketName_Nuno;

                        var storageObject =
                            await storageClient.UploadObjectAsync(
                                _options.GCPStorageBucketName_Nuno,
                                fileNameToSave, fileToUpload.ContentType,
                                memoryStream);

                        _logger.LogInformation(
                            $"Uploaded File Async: {fileToUpload.FileName} to {fileNameToSave} into storage {_options.GCPStorageBucketName_Nuno}");

                        return await Task.FromResult(storageObject.MediaLink);
                    }

                    //var uploadFile = _googleCredentials.CreateScoped(scopes: _options.Scopes).UnderlyingCredential as Google.Apis.Auth.OAuth2.GoogleCredential;
                    //var storageClient = Google.Cloud.Storage.V1.StorageClient.Create(uploadFile);
                    //var bucketName = _options.BucketName;
                    //var storageObject = storageClient.UploadObject(bucketName, fileNameToSave, null, memoryStream);

                    //return await Task.FromResult(storageObject.MediaLink);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex.Message}");

                //return $"Error while uploading file {fileNameToSave} {ex.Message}";

                return await Task.FromResult(
                    $"Error while uploading file {fileNameToSave} {ex.Message}");
            }
        }
    }
}