using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SuperShop.Web.Data.Entities;

public class OrderDetailTemp : IEntity
{
    [Required] public User User { get; set; }


    // [OnDelete(DeleteBehavior.NoAction)]
    // [OnUpdate(DeleteBehavior.NoAction)]
    // [ForeignKey(nameof(Product))]
    // public int ProductId { get; set; }

    // [System.ComponentModel.DataAnnotations.Schema.Column(Order = 1)]
    // [ForeignKey(nameof(ProductId))]
    [Required] public Product Product { get; set; }


    // [Required]
    // [Column(TypeName = "decimal(18,2)")]
    [DataType(DataType.Currency)]
    [Precision(18, 2)]
    [DisplayFormat(DataFormatString = "{0:C2}")]
    public decimal Price { get; set; }


    // [Required]
    // [DataType(DataType.Custom)]
    [DisplayFormat(DataFormatString = "{0:N2}")]
    public double Quantity { get; set; }


    [DataType(DataType.Currency)]
    [Precision(18, 2)]
    [DisplayFormat(DataFormatString = "{0:C2}")]
    public decimal Value => (decimal) Quantity * Price;

    [Key]
    // [Required]
    // [Column(Order = 0)]
    public int Id { get; set; }


    [DisplayName("Was Deleted?")] public bool WasDeleted { get; set; }
}