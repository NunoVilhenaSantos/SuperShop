using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SuperShop.Web.Data.Entity;

public class OrderDetail : IEntity
{
    [Key] [Required] public int Id { get; set; }


    [Required]
    [DisplayName("Was Deleted?")]
    public bool WasDeleted { get; set; }


    [Required] public Product Product { get; set; }


    [Required]
    // [Column(TypeName = "decimal(18,2)")]
    [Precision(18, 2)]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }


    [Required]
    [DataType(DataType.Custom)]
    [DisplayFormat(DataFormatString = "{0:N2}")]
    public double Quantity { get; set; }


    [DataType(DataType.Currency)]
    public decimal Value => (decimal) Quantity * Price;
}