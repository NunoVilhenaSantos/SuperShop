using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SuperShop.Web.Data.Entities;

public class Country : IEntity
{
    [Required]
    [DisplayName("Country")]
    [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters.")]
    public string Name { get; set; }


    public ICollection<City> Cities { get; set; }


    [DisplayName("Number of Cities")]
    public int NumberCities => Cities?.Count ?? 0;

    [Key] [Required] public int Id { get; set; }


    [Required] public bool WasDeleted { get; set; }
}