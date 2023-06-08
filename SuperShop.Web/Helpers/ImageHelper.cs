using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SuperShop.Web.Helpers
{
    public class ImageHelper : IImageHelper
    {
        public async Task<string> UploadImageAsync(
            IFormFile imageFile, string folder)
        {
            var guid = Guid.NewGuid().ToString();
            var fileName = guid + ".png";

            //var filePath =
            //    Directory.GetCurrentDirectory() +
            //    $"\\wwwroot\\images\\{folder}\\{fileName}";

            var filePath =
                Directory.GetCurrentDirectory() +
                $"\\wwwroot\\images\\{folder}\\";

            Directory.CreateDirectory(filePath);

            await using var stream =
                new FileStream(
                    filePath + fileName, FileMode.Create, FileAccess.ReadWrite);

            await imageFile.CopyToAsync(stream);

            return $"~/images/{folder}/{fileName}";
        }
    }
}