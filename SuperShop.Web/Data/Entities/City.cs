using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using RequiredMemberAttribute =
    System.Runtime.CompilerServices.RequiredAttributeAttribute;

namespace SuperShop.Web.Data.Entities;

public class City : IEntity
{
    [Required]
    [DisplayName("City")]
    [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters.")]
    public required string Name { get; set; }


    [Key] [Required] public int Id { get; set; }


    [Required] public required bool WasDeleted { get; set; }
}