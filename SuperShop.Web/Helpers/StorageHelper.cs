using System;
using System.IO;
using System.Threading.Tasks;
using Azure;
using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace SuperShop.Web.Helpers
{
    public class StorageHelper : IStorageHelper
    {
        private readonly string _azureBlobKey_1;
        private readonly string _azureBlobKey_2;
        private readonly AzureKeyCredential _azureKeyCredential;
        private readonly AzureSasCredential _azureSasCredential;
        private readonly IConfiguration _configuration;

        private string _awsStorageKey1;
        private string _awsStorageKey2;

        private string _gcpStorageKey1;
        private string _gcpStorageKey2;

        public StorageHelper(IConfiguration configuration)
        {
            _configuration = configuration;

            _azureBlobKey_1 = _configuration["Storages:AzureBlobKey-1"];
            _azureBlobKey_2 = _configuration["Storages:AzureBlobKey-2"];
            _gcpStorageKey1 = _configuration["Storages:GCPStorageKey1"];
            _gcpStorageKey2 = _configuration["Storages:GCPStorageKey2"];
            _awsStorageKey1 = _configuration["Storages:AWSStorageKey1"];
            _awsStorageKey2 = _configuration["Storages:AWSStorageKey2"];

            _azureKeyCredential = new AzureKeyCredential(_azureBlobKey_1);
            _azureSasCredential = new AzureSasCredential(_azureBlobKey_2);


            // Retrieve the connection string for use with the application.
            var connectionString =
                Environment.GetEnvironmentVariable(
                    "AZURE_STORAGE_CONNECTION_STRING");

            // Create a BlobServiceClient object
            var blobServiceClient = new BlobServiceClient(_azureBlobKey_1);

            // "DefaultEndpointsProtocol=https;" +
            //     "AccountName=storagesuper;" +
            //     "AccountKey=your_storage_account_key;" +
            //     "EndpointSuffix=core.windows.net");
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


        private async Task<Guid> UploadStreamAsync(
            Stream stream, string bucketName)
        {
            var name = Guid.NewGuid();


            // Get a reference to a container named "sample-container" and then create it
            var blobContainerClient =
                new BlobContainerClient(
                    _configuration["Storages:AzureBlobKey-1"],
                    bucketName);


            // Get a reference to a blob named "sample-file" in a container named "sample-container"
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
            containerName = "quickstartblobs" + Guid.NewGuid();

            // Create the container and return a container client object
            BlobContainerClient containerClient =
                await blobServiceClient.CreateBlobContainerAsync(
                    containerName);

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
    }
}