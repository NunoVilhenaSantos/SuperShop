using System.ComponentModel;
using Microsoft.Build.Framework;

namespace SuperShop.Web.Models;

public class ChangeUserViewModel
{
    [Required] [DisplayName("First Name")] public string FirstName { get; set; }


    [Required] [DisplayName("Last Name")] public string LastName { get; set; }
}