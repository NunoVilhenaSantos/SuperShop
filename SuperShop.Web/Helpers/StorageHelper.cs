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
        IOptions<GCPConfigOptions> options,
        ILogger<CloudStorageService> logger
    )
    {
        _configuration = configuration;
        _logger = logger;


        // _azureBlobKey_1 = _configuration["Storages:AzureBlobKeyNuno"];
        // _azureBlobKey_2 = _configuration["Storages:AzureBlobKeyRuben"];
        // "AzureBlobKeyNuno": "",
        // "AzureBlobKeyRuben": "",
        // "AzureBlobKeyLicinio": "",
        // "AzureBlobKeyJorge": "",
        // "AzureBlobKeyJoel": "",
        // "AzureBlobKey-6": ""
        //_azureKeyCredential = new AzureKeyCredential(_azureBlobKey_1);
        //_azureSasCredential = new AzureSasCredential(_azureBlobKey_2);
        //_awsStorageKey1 = _configuration["Storages:AWSStorageKey1"];
        //_awsStorageKey2 = _configuration["Storages:AWSStorageKey2"];
        // Retrieve the connection string for use with the application.
        // var connectionString =
        //     Environment.GetEnvironmentVariable(
        //         "AZURE_STORAGE_CONNECTION_STRING");
        // Create a BlobServiceClient object
        // var blobServiceClient = new BlobServiceClient(_azureBlobKey_1);
        // "DefaultEndpointsProtocol=https;" +
        //     "AccountName=storagesuper;" +
        //     "AccountKey=your_storage_account_key;" +
        //     "EndpointSuffix=core.windows.net")
        // ;

        // Create the container and return a container client object
        // Create a unique name for the container
        // var gcpStorageFileNuno = _configuration["GCPStorageAuthFile_Nuno"];
        // _gcpStorageBucketNuno = _configuration["GCPStorageBucketName_Nuno"];
        // _googleCredentialsNuno =
        //     GoogleCredential.FromFile(gcpStorageFileNuno);


        //_gcpStorageFileJorge = _configuration["GCPStorageAuthFile_Jorge"];
        //_gcpStorageBucketJorge =
        //    _configuration["GCPStorageBucketName_Jorge"];
        //_googleCredentialsNuno =
        //    GoogleCredential.FromFile(_gcpStorageFileJorge);


        // _options = options.Value;
        // _logger = logger;
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


    public async Task<string> UploadFileAsyncToGCP(IFormFile fileToUpload,
        string fileNameToSave)
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
                        $"{fileToUpload.FileName} to " +
                        $"{fileNameToSave} into storage {_configuration[""]}");

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


    public async Task<BlobServiceClient> CreateContainer(
        string containerName)
    {
        // TODO: Replace <storage-account-name> with your actual storage account name
        var blobServiceClient = new BlobServiceClient(
            new Uri("https://<storage-account-name>.blob.core.windows.net"),
            new DefaultAzureCredential());

        // Create a unique name for the container
        containerName += "quickstartblobs" + Guid.NewGuid();

        // Create the container and return a container client object
        // BlobContainerClient containerClient =
        //     await blobServiceClient.CreateBlobContainerAsync(
        //         containerName);

        return blobServiceClient;
    }


    public Task<bool> CopyFileToBlob()
    {
        // Get a connection string to our Azure Storage account.
        // You can obtain your connection string from the Azure Portal
        // (click Access Keys under Settings
        //  in the Portal Storage account blade)
        // or using the Azure CLI with:
        //
        //     az storage account show-connection-string
        //     --name <account_name> --resource-group <resource_group>
        //
        // And you can provide the connection string to your application
        // using an environment variable.

        var connectionString = _configuration["Storages:AzureBlobKey1"];
        var containerName = "sample-container";
        var blobName = "sample-blob";
        var filePath = "sample-file";

        // Get a reference to a container named "sample-container" and then create it
        var container =
            new BlobContainerClient(connectionString, containerName);

        container.Create();

        // Get a reference to a blob named "sample-file" in a container named "sample-container"
        var blob = container.GetBlobClient(blobName);

        // Upload local file
        blob.Upload(filePath);

        return Task.FromResult(true); // "Uploaded file to blob storage.";
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

    private readonly GCPConfigOptions _options;

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