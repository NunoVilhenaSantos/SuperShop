using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SuperShop.Web.Models;

public class RegisterNewUserViewModel
{
    [System.ComponentModel.DataAnnotations.Required]
    [DisplayName("First Name")]
    public string FirstName { get; set; }


    [System.ComponentModel.DataAnnotations.Required]
    [DisplayName("Last Name")]
    public string LastName { get; set; }


    [System.ComponentModel.DataAnnotations.Required]
    [DataType(DataType.EmailAddress)]
    public string Username { get; set; }


    [System.ComponentModel.DataAnnotations.Required]
    [DataType(DataType.Password)]
    [MinLength(6)]
    public string Password { get; set; }


    [System.ComponentModel.DataAnnotations.Required]
    [DataType(DataType.Password)]
    [Compare("Password")]
    [MinLength(6)]
    public string ConfirmPassword { get; set; }
}