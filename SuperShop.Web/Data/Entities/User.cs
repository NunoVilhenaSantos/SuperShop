using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SuperShop.Web.Data.Entities;

public class User : IdentityUser
{
    [Display(Name = "First Name")] public string FirstName { get; set; }


    [Display(Name = "Last Name")] public string LastName { get; set; }


    [Display(Name = "Full Name")]
    public string FullName => $"{FirstName} {LastName}";
}