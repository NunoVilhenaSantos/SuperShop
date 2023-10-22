using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Text.RegularExpressions;
using SuperShop.Web.Helpers;

namespace SuperShop.Web.Data.Entities;

public class Product : IEntity
{
    [Required]
    [MaxLength(50,
        ErrorMessage =
            "The field {0} can contain {1} characters of length")]
    public required string Name { get; set; }


    // public string Description { get; set; }
    // public string ProductId { get; set; } = string.Empty;


    [Required]
    [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
    [Column(TypeName = "decimal(18, 2)")]
    public required decimal Price { get; set; }


    [Display(Name = "Image")] public string ImageUrl { get; set; }

    public string ImageFullUrl => string.IsNullOrEmpty(ImageUrl)
        ? "https://supershop.blob.core.windows.net/placeholders/noimage.png"
        : Regex.Replace(ImageUrl, @"^~/products/images/",
         "https://supershop.blob.core.windows.net/products/");


    [Display(Name = "Image ID")] public Guid ImageId { get; set; }

    public string ImageIdFullUrl => ImageId == Guid.Empty
        ? "https://supershop.blob.core.windows.net/placeholders/noimage.png"
        : Path.Combine("https://supershop.blob.core.windows.net/", "products",
            ImageId.ToString());



    [Display(Name = "Last Purchase")]
    public DateTime? LastPurchase { get; set; }

    [Display(Name = "Last Sale")] public DateTime? LastSale { get; set; }


    [Required]
    [Display(Name = "Is Available")]
    public required bool IsAvailable { get; set; }


    [Required]
    [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
    public required double Stock { get; set; }


    [Required] public required User User { get; set; }

    [Key] [Required] public int Id { get; set; }

    [Required] public required bool WasDeleted { get; set; }
}