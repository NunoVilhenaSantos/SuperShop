using Microsoft.AspNetCore.Http;
using System.ComponentModel;

namespace SuperShop.Web.Models
{
    public class UserViewModel
    {
        [DisplayName("Image")] public IFormFile ImageFile { get; set; }
    }
}
