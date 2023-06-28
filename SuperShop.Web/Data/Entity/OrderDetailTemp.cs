using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SuperShop.Web.Data.Entity;

public class OrderDetailTemp : IEntity
{
    [Required] public User User { get; set; }


    [Required] public Product Product { get; set; }


    [Required]
    [DataType(DataType.Currency)]
    // [Column(TypeName = "decimal(18,2)")]
    [Precision(18, 2)]
    public decimal Price { get; set; }


    [Required]
    [DataType(DataType.Custom)]
    [DisplayFormat(DataFormatString = "{0:N2}")]
    public double Quantity { get; set; }


    [DataType(DataType.Currency)]
    public decimal Value => (decimal) Quantity * Price;

    [Key] [Required] public int Id { get; set; }


    [Required]
    [DisplayName("Was Deleted?")]
    public bool WasDeleted { get; set; }
}