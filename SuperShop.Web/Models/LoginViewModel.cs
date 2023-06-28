using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SuperShop.Web.Models;

public class LoginViewModel
{
    [Required] [EmailAddress] public string Username { get; set; }


    [Required]
    [MinLength(6)]
    [DataType(DataType.Password)]
    public string Password { get; set; }


    [Required]
    [DisplayName("Remember Me?")]
    public bool RememberMe { get; set; }
}