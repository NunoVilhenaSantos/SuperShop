using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SuperShop.Web.Data.Entities;

public class OrderDetail : IEntity
{
    [Required] public Order Order { get; set; }


    [Required] public Product Product { get; set; }


    // [Required]
    // [Column(TypeName = "decimal(18,2)")]
    [DataType(DataType.Currency)]
    [Precision(18, 2)]
    public decimal Price { get; set; }


    // [Required]
    // [DataType(DataType.Custom)]
    [DisplayFormat(DataFormatString = "{0:N2}")]
    public double Quantity { get; set; }


    [DataType(DataType.Currency)]
    [Precision(18, 2)]
    public decimal Value => (decimal) Quantity * Price;

    [Key]
    // [Required]
    // [Column(Order = 0)]
    public int Id { get; set; }


    [Required]
    [DisplayName("Was Deleted?")]
    public bool WasDeleted { get; set; }
}