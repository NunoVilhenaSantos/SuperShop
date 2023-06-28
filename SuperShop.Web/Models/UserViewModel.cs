using System.ComponentModel;
using Microsoft.AspNetCore.Http;

namespace SuperShop.Web.Models;

public class UserViewModel
{
    [DisplayName("Image")] public IFormFile ImageFile { get; set; }
}