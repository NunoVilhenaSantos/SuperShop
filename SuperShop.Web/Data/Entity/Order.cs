using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SuperShop.Web.Data.Entity;

public class Order : IEntity

{
    [Required] public User User { get; set; }


    [Required]
    [DisplayName("Order Date")]
    [DataType(DataType.DateTime)]
    // [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
    public Product OrderDate { get; set; }


    [Required]
    [DisplayName("Delivery Date")]
    [DataType(DataType.DateTime)]
    // [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
    public Product DeliveryDate { get; set; }


    public IEnumerable<OrderDetail> Items { get; set; }


    [DataType(DataType.Custom)]
    [DisplayFormat(DataFormatString = "{0:N2}")]
    public int Count => Items?.Count() ?? 0;


    [DataType(DataType.Custom)]
    [DisplayFormat(DataFormatString = "{0:N2}")]
    public double Quantity => Items?.Sum(i => i.Quantity) ?? 0;


    [DataType(DataType.Currency)]
    public decimal Total => Items?.Sum(i => i.Value) ?? 0;

    [Key] [Required] public int Id { get; set; }


    [Required]
    [DisplayName("Was Deleted?")]
    public bool WasDeleted { get; set; }


    // [Required] public OrderStatus OrderStatus { get; set; }
}