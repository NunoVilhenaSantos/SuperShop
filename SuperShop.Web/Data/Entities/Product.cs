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
        ? "https://supershopweb.blob.core.windows.net/noimage/noimage.png"
        : Regex.Replace(ImageUrl, @"^~/products/images/",
            "https://myleasingnunostorage.blob.core.windows.net/owners/");


    [Display(Name = "Image ID")] public Guid ImageId { get; set; }

    public string ImageIdFullUrl => ImageId == Guid.Empty
        // ? "https://supershopweb.blob.core.windows.net/noimage/noimage.png"
        // : "https://storage.googleapis.com/storage-ruben/" +
        //   "/products/" + ImageId;
        ? "https://supershopcet77.azurewebsites.net/images/noimage.png"
        : Path.Combine(StorageHelper.GcpStoragePublicUrl, "products",
            ImageId.ToString());



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


    [Required]
    [Display(Name = "Is Available")] public required bool IsAvailable { get; set; }


    [Required]
    [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
    public required double Stock { get; set; }


    [Required] public required User User { get; set; }

    [Key] [Required] public int Id { get; set; }

    [Required] public required bool WasDeleted { get; set; }
}