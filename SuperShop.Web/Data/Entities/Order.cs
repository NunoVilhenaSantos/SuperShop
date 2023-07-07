using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SuperShop.Web.Data.Entities;

public class Order : IEntity

{
    // [Required] public OrderStatus OrderStatus { get; set; }
    [Required]
    [DisplayName("Order Date")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}",
        ApplyFormatInEditMode = false)]
    public DateTime OrderDate { get; set; }


    [DisplayName("Delivery Date")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}",
        ApplyFormatInEditMode = false)]
    public DateTime? DeliveryDate { get; set; }


    [Required] public User User { get; set; }


    public IEnumerable<OrderDetail> Items { get; set; }
    // public ICollection<OrderDetail> Items { get; set; }
    // = new List<OrderDetail>();


    // [DataType(DataType.Custom)]
    [DisplayFormat(DataFormatString = "{0:N0}")]
    public int Lines => Items?.Count() ?? 0;


    // [DataType(DataType.Custom)]
    [DisplayFormat(DataFormatString = "{0:N2}")]
    public double Quantity => Items?.Sum(i => i.Quantity) ?? 0;


    [DataType(DataType.Currency)]
    [DisplayFormat(DataFormatString = "{0:C2}")]
    public decimal Total => Items?.Sum(i => i.Value) ?? 0;


    [DisplayName("Order Date")]
    [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy HH:mm tt}",
        ApplyFormatInEditMode = false)]
    public DateTime? OrderDateLocal => OrderDate.ToLocalTime();


    [DisplayName("Delivery Date")]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}",
        ApplyFormatInEditMode = false)]
    public DateTime? DeliveryDateLocal =>
        DeliveryDate.GetValueOrDefault().ToLocalTime();


    [Key]
    // [Required]
    public int Id { get; set; }


    [Required]
    [DisplayName("Was Deleted?")]
    public bool WasDeleted { get; set; }
}