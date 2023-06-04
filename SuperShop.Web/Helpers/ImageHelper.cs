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
            var fileName = guid + ".jpg";

            var filePath =
                Directory.GetCurrentDirectory() +
                $"\\wwwroot\\images\\{folder}\\{fileName}";

            await using var stream =
                new FileStream(
                    filePath, FileMode.Create, FileAccess.ReadWrite);

            await imageFile.CopyToAsync(stream);

            return $"~/images/{folder}/{fileName}";
        }
    }
}