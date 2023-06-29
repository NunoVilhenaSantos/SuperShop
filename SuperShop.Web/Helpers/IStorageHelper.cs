using System;
using System.IO;
using System.Threading.Tasks;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;

namespace SuperShop.Web.Helpers;

public interface IStorageHelper
{
    Task<bool> CopyFile(
        string sourceBucketName = "source-bucket-name",
        string sourceObjectName = "source-file",
        string destBucketName = "destination-bucket-name",
        string destObjectName = "destination-file-name")
    {
        var storage = StorageClient.Create();
        storage.CopyObject(
            sourceBucketName,
            sourceObjectName,
            destBucketName,
            destObjectName);

        Console.WriteLine(
            "Copied " +
            $"{sourceBucketName}/{sourceObjectName}" +
            " to " +
            $"{destBucketName}/{destObjectName}.");

        return Task.FromResult(
            storage.GetNotification(
                sourceBucketName, sourceObjectName) != null);
    }


    Task<Guid> UploadFileToGCP(
        string bucketName = "your-unique-bucket-name",
        string localPath = "my-local-path/my-file-name",
        string objectName = "my-file-name")
    {
        var storage = StorageClient.Create();

        using var fileStream = File.OpenRead(localPath);

        storage.UploadObject(bucketName, objectName, null, fileStream);

        Console.WriteLine($"Uploaded {objectName}.");

        return Task.FromResult(Guid.NewGuid());
    }

    Task<string> UploadFileAsyncToGCP(IFormFile fileToUpload,
        string fileNameToSave);

    // [END storage_stream_file_upload]
    // [END storage_upload_file]

    Task<Guid> UploadStorageAsync(IFormFile file, string bucketName);

    Task<Guid> UploadStorageAsync(byte[] file, string bucketName);

    Task<Guid> UploadStorageAsync(string file, string bucketName);
}