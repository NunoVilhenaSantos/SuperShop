using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SuperShop.Web.Data.Entity;

public class OrderDetailTemp : IEntity
{
    [Required] public User User { get; set; }


    // [OnDelete(DeleteBehavior.NoAction)]
    // [OnUpdate(DeleteBehavior.NoAction)]
    // [Required]
    // [ForeignKey(nameof(Product))]
    public int ProductId { get; set; }

    // [System.ComponentModel.DataAnnotations.Schema.Column(Order = 1)]
    [ForeignKey(nameof(ProductId))]
    [Required]
    public Product Product { get; set; }


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
    [Precision(18, 2)]
    public decimal Value => (decimal) Quantity * Price;

    [Key] [Required] [Column(Order = 0)] public int Id { get; set; }


    [DisplayName("Was Deleted?")] public bool WasDeleted { get; set; }
}