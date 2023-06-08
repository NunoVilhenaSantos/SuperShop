using System;
using System.Threading.Tasks;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;

namespace SuperShop.Web.Helpers
{
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

        Task<Guid> UploadStorageAsync(IFormFile file, string bucketName);

        Task<Guid> UploadStorageAsync(byte[] file, string bucketName);

        Task<Guid> UploadStorageAsync(string file, string bucketName);
    }
}