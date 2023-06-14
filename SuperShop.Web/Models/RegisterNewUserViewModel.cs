using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SuperShop.Web.Models;

public class RegisterNewUserViewModel
{
    [Microsoft.Build.Framework.Required]
    [DisplayName("First Name")]
    public string FirstName { get; set; }


    [Microsoft.Build.Framework.Required]
    [DisplayName("Last Name")]
    public string LastName { get; set; }


    [Microsoft.Build.Framework.Required]
    [DataType(DataType.EmailAddress)]
    public string Username { get; set; }


    [Microsoft.Build.Framework.Required]
    [DataType(DataType.Password)]
    [MinLength(6)]
    public string Password { get; set; }


    [Microsoft.Build.Framework.Required]
    [DataType(DataType.Password)]
    [Compare("Password")]
    [MinLength(6)]
    public string ConfirmPassword { get; set; }
}