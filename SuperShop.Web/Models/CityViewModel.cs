using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SuperShop.Web.Models;

public class CityViewModel
{
    public int CountryId { get; set; }


    public int CityId { get; set; }


    [Required]
    [DisplayName("City")]
    [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters.")]
    public string Name { get; set; }
}