using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SuperShop.Web.Models;

public class ChangePasswordViewModel
{
    [System.ComponentModel.DataAnnotations.Required]
    [DisplayName("Current Password")]
    [DataType(DataType.Password)]
    public string OldPassword { get; set; }


    [System.ComponentModel.DataAnnotations.Required]
    [DataType(DataType.Password)]
    [MinLength(6)]
    public string NewPassword { get; set; }


    [System.ComponentModel.DataAnnotations.Required]
    [DataType(DataType.Password)]
    [Compare("NewPassword")]
    [MinLength(6)]
    public string ConfirmPassword { get; set; }
}