using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuperShop.Web.Data.Entity
{
    public class Product : IEntity
    {
        [Required]
        [MaxLength(50,
            ErrorMessage =
                "The field {0} can contain {1} characters of length")]
        public string Name { get; set; }


        // public string Description { get; set; }
        // public string ProductId { get; set; } = string.Empty;


        [DisplayFormat(DataFormatString = "{0:C2}",
            ApplyFormatInEditMode = false)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }


        [Display(Name = "Image")] public string ImageUrl { get; set; }

        public string ImageFullUrl => string.IsNullOrEmpty(ImageUrl)
            ? "https://supershopwebtpsicet77.azurewebsites.net/" +
              "images/no-stamp-1.png"
            : "https://supershopwebtpsicet77.azurewebsites.net/" +
              $"{ImageUrl[1..]}";


        [Display(Name = "Image ID")] public Guid ImageId { get; set; }

        public string ImageFullIdUrl => ImageId == Guid.Empty
            ? "https://supershopwebtpsicet77.azurewebsites.net/" +
              "images/no-stamp-1.png"
            : "https://supershopwebtpsicet77.blob.core.windows.net/" +
              $"{GetType().Name.ToLower()}s/{ImageId}";


        // [Display(Name = "Thumbnail")]
        // public string ImageThumbnailUrl { get; set; }
        //
        // public string ImageThumbnailFullUrl =>
        //     string.IsNullOrEmpty(ImageThumbnailUrl)
        //         ? null
        //         : $"https://supermarketapi.azurewebsites.net{ImageThumbnailUrl[1..]}";


        [Display(Name = "Last Purchase")]
        public DateTime? LastPurchase { get; set; }

        [Display(Name = "Last Sale")] public DateTime? LastSale { get; set; }

        [Display(Name = "Is Available")] public bool IsAvailable { get; set; }


        [DisplayFormat(DataFormatString = "{0:N2}",
            ApplyFormatInEditMode = false)]
        public double Stock { get; set; }


        public User User { get; set; }

        [Key] public int Id { get; set; }
    }
}