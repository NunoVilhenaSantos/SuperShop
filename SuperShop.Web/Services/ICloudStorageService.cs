using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SuperShop.Web.Services;

public interface ICloudStorageService
{
    // Task<string> GetFileAsync(string bucketName, string fileNameToRead);


    // Task<string> UploadFileAsync(
    //     string bucketName, string fileNameToUpload,
    //     string filePath, string contentType = null);


    // Task<string> UploadFileAsync(
    //     string bucketName, string fileNameToUpload,
    //     byte[] data, string contentType = null);


    // Task<string> UploadFileAsync(
    //     string bucketName, IFormFile fileNameToUpload,
    //     string filePath, string contentType = null);


    // Task<string> UploadFileAsync(
    //     string bucketName, IFormFile fileNameToUpload,
    //     byte[] data, string contentType = null);


    // Task<bool> DeleteFileAsync(string bucketName, string fileNameToDelete);


    Task<string> GetSignedUrlAsync(
        string fileNameToRead, int timeOutInMinutes = 2);

    Task<string> UploadFileAsync(
        IFormFile fileToUpload, string fileNameToSave);

    Task<bool> DeleteFileAsync(string fileNameToDelete);
}