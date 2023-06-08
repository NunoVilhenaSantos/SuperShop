using System.Threading.Tasks;

namespace SuperShop.Web.Services
{
    public interface ICloudStorageService
    {

        //Task<string> GetFileAsync(string bucketName, string fileNameToRead);


        //Task<string> UploadFileAsync(string bucketName, string fileNameToUpload, string filePath, string contentType = null);
        //Task<string> UploadFileAsync(string bucketName, string fileNameToUpload, byte[] data, string contentType = null);


        //Task<string> UploadFileAsync(string bucketName, Microsoft.AspNetCore.Http.IFormFile fileNameToUpload, string filePath, string contentType = null);
        //Task<string> UploadFileAsync(string bucketName, Microsoft.AspNetCore.Http.IFormFile fileNameToUpload, byte[] data, string contentType = null);


        //Task<bool> DeleteFileAsync(string bucketName, string fileNameToDelete);


        Task<string> GetSignedUrlAsync(string fileNameToRead, int timeOutInMinutes = 2);
        Task<string> UploadFileAsync(Microsoft.AspNetCore.Http.IFormFile fileToUpload, string fileNameToSave);
        Task<bool> DeleteFileAsync(string fileNameToDelete);
    }
}