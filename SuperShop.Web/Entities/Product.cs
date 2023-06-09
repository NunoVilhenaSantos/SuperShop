#nullable disable

using System;

namespace SuperShop.Web.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public Guid ImageId { get; set; }
        public string ImageIdGcp { get; set; }
        public Guid ImageIdAws { get; set; }
        public DateTime? LastPurchase { get; set; }
        public DateTime? LastSale { get; set; }
        public bool IsAvailable { get; set; }
        public double Stock { get; set; }
        public string UserId { get; set; }

        public virtual AspNetUser User { get; set; }
    }
}