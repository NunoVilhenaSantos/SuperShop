using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace SuperShop.Web.Helpers;

public class StorageHelper : IStorageHelper
{
    // "GCPStorageAuthFile_Jorge": "C:\\Users\\nunov\\Downloads\\GCP\\lateral-isotope-388820-755e381a94ef-jorge.json",
    // "GCPStorageAuthFile_Nuno": "C:\\Users\\nunov\\Downloads\\GCP\\lateral-isotope-388820-f36a4ce5137c-nuno.json",
    // "GCPStorageBucketName_Jorge": "supershoptpsicet77-jorge",
    // "GCPStorageBucketName_Nuno": "supershoptpsicet77-nuno"

    private readonly IConfiguration _configuration;
    // private readonly ILogger<CloudStorageService> _logger;

    // private readonly string _azureBlobKey_1;
    // private readonly string _azureBlobKey_2;

    private readonly GoogleCredential _googleCredentials;

    public StorageHelper(
        IConfiguration configuration
        // ILogger<CloudStorageService> logger
    )
    {
        _configuration = configuration;

        var gcpStorageFileAccess =
            _configuration["GCPStorageAuthFile_Nuno"];

        _googleCredentials =
            GoogleCredential.FromFile(gcpStorageFileAccess);
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


    public async Task<Guid> UploadFileAsyncToGcp(IFormFile fileToUpload,
        string fileNameToSave)
    {
        throw new NotImplementedException();
    }

    public async Task<Guid> UploadFileAsyncToGcp(
        string fileToUpload, string fileNameInBucket,
        string gcpStorageBucketName = "supershoptpsicet77-jorge")
    {
        try
        {
            // create a memory stream from the file bytes
            using (var memoryStream =
                   new MemoryStream(File.ReadAllBytes(fileToUpload)))
            {
                // Create storage client using the credentials file.
                using (var storageClient =
                       await StorageClient.CreateAsync(_googleCredentials))
                {
                    gcpStorageBucketName = "supershoptpsicet77-nuno";
                    var uniqueFileName = Guid.NewGuid();
                    fileNameInBucket += uniqueFileName;


                    await DeleteFileAsyncFromGcp(
                        fileNameInBucket, gcpStorageBucketName);


                    // Log information - Begin file upload
                    Log.Logger.Information(
                        "Uploading file: " +
                        "{File} to {Name} in storage bucket {GcpStorage}",
                        fileToUpload,
                        fileNameInBucket,
                        gcpStorageBucketName);


                    // Upload the file to storage
                    var storageObject =
                        await storageClient.UploadObjectAsync(
                            gcpStorageBucketName, fileNameInBucket,
                            null, memoryStream);


                    // Log information - File upload complete
                    Log.Logger.Information(
                        "File uploaded successfully: " +
                        "{File} to {Name} in storage bucket {GcpStorage}",
                        fileToUpload,
                        fileNameInBucket,
                        gcpStorageBucketName);


                    return await Task.FromResult(uniqueFileName);
                }
            }
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex,
                "Error while uploading file: {File}",
                fileToUpload);
            return await Task.FromResult(Guid.Empty);
        }
    }


    public async Task<bool> DeleteFileAsyncFromGcp(
        string fileNameInBucket,
        string gcpStorageBucketName)
    {
        // Create storage client using the credentials file.
        using var storageClient =
            await StorageClient.CreateAsync(_googleCredentials);

        var assetExists =
            await storageClient.GetObjectAsync(
                gcpStorageBucketName,
                fileNameInBucket);

        switch (assetExists)
        {
            case null:
                return true;

            default:
                await storageClient.DeleteObjectAsync(assetExists);

                assetExists =
                    await storageClient.GetObjectAsync(
                        gcpStorageBucketName,
                        fileNameInBucket);

                return assetExists is null;
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
}