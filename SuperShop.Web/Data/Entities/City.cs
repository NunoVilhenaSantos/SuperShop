using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SuperShop.Web.Data.Entities;

public class City: IEntity
{
    [Key] [Required] public int Id { get; set; }


    [Required] public bool WasDeleted { get; set; }


    [Required]
    [DisplayName("City")]
    [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters.")]
    public string Name { get; set; }
}