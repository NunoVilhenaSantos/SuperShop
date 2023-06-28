using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SuperShop.Web.Models;

public class ChangePasswordViewModel
{
    [Required]
    [DisplayName("Current Password")]
    [DataType(DataType.Password)]
    public string OldPassword { get; set; }


    [Required]
    [DataType(DataType.Password)]
    [MinLength(6)]
    public string NewPassword { get; set; }


    [Required]
    [DataType(DataType.Password)]
    [Compare("NewPassword")]
    [MinLength(6)]
    public string ConfirmPassword { get; set; }
}