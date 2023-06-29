using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Storage.Blobs;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SuperShop.Web.Services;
using SuperShop.Web.Utils.ConfigOptions;

namespace SuperShop.Web.Helpers;

public class StorageHelper : IStorageHelper
{
    public StorageHelper(
        IConfiguration configuration,
        ILogger<CloudStorageService> logger
    )
    {
        _configuration = configuration;
        _logger = logger;
    }


    public async Task<Guid> UploadStorageAsync(
        IFormFile file, string bucketName)
    {
        var stream = file.OpenReadStream();
        return await UploadStreamAsync(stream, bucketName);
    }


    public async Task<Guid> UploadStorageAsync(
        byte[] file, string bucketName)
    {
        var stream = new MemoryStream(file);
        return await UploadStreamAsync(stream, bucketName);
    }


    public async Task<Guid> UploadStorageAsync(
        string file, string bucketName)
    {
        var stream = File.OpenRead(file);
        return await UploadStreamAsync(stream, bucketName);
    }


    public async Task<string> UploadFileAsyncToGCP(
        IFormFile fileToUpload, string fileNameToSave)
    {
        try
        {
            _logger.LogInformation(
                $"Uploading File Async: {fileToUpload.FileName} to " +
                $"{fileNameToSave} into storage " +
                $"{_configuration["GCPStorageBucketName_Nuno"]}");


            using (var memoryStream = new MemoryStream())
            {
                await fileToUpload.CopyToAsync(memoryStream);

                // create storage client using the credentials file.
                using (var storageClient =
                       StorageClient.Create(_googleCredentialsNuno))
                {
                    //var bucketName = _options.GCPStorageBucketName_Nuno;

                    var storageObject =
                        await storageClient.UploadObjectAsync(
                            _gcpStorageBucketNuno, fileNameToSave,
                            fileToUpload.ContentType, memoryStream);

                    _logger.LogInformation(
                        $"Uploaded File Async: " +
                        $"{0} to {1} into storage {2}",
                        fileToUpload.FileName, fileNameToSave,
                        _configuration["GCPStorageBucketName_Nuno"]);

                    return await Task.FromResult(storageObject.MediaLink);
                }
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


    private async Task<Guid> UploadStreamAsync(
        Stream stream, string bucketName)
    {
        var name = Guid.NewGuid();


        // Get a reference to a container named "sample-container"
        // and then create it
        var blobContainerClient =
            new BlobContainerClient(
                _configuration["Storages:AzureBlobKeyNuno"],
                bucketName);


        // Get a reference to a blob named "sample-file"
        // in a container named "sample-container"
        var blobClient =
            blobContainerClient.GetBlobClient(name.ToString());


        // Check if the container already exists
        bool containerExists = await blobContainerClient.ExistsAsync();


        // Create the container if it doesn't exist
        if (!containerExists) await blobContainerClient.CreateAsync();

        // Perform any additional setup or
        // configuration for the container if needed
        // Upload local file
        await blobClient.UploadAsync(stream);

        return name; // "Uploaded file to blob storage.";
    }


    #region Fields

    private readonly IConfiguration _configuration;


    #region Azure

    // private readonly string _azureBlobKey_1;
    // private readonly string _azureBlobKey_2;
    // private readonly AzureKeyCredential _azureKeyCredential;
    // private readonly AzureSasCredential _azureSasCredential;

    #endregion


    #region AWS

    // private string _awsStorageKey1;
    // private string _awsStorageKey2;

    #endregion


    #region GCP

    // private readonly GCPConfigOptions _options;

    private readonly ILogger<CloudStorageService> _logger;
    //private readonly Google.Apis.Auth.OAuth2.GoogleCredential _googleCredentials;

    private readonly string _gcpStorageBucketNuno;
    private readonly GoogleCredential _googleCredentialsNuno;

    // private readonly string _gcpStorageFileJorge;
    // private string _gcpStorageBucketJorge;
    // private GoogleCredential _googleCredentialsJorge;

    #endregion

    #endregion
}