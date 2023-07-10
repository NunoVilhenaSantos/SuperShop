using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SuperShop.Web.Data.Entities;

public class User : IdentityUser
{
    [MaxLength(50,
        ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
    [DisplayName("First Name")]
    public string FirstName { get; set; }


    [MaxLength(50,
        ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
    [DisplayName("Last Name")]
    public string LastName { get; set; }


    [DisplayName("Full Name")]
    public string FullName => $"{FirstName} {LastName}";


    [MaxLength(100,
        ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
    public string Address { get; set; }


    [Display(Name = "Country")] public int CountryId { get; set; }


    public City City { get; set; }
}