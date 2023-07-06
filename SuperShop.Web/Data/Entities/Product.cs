﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace SuperShop.Web.Data.Entities;

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
        ? "https://supershopweb.blob.core.windows.net/noimage/noimage.png"
        : Regex.Replace(ImageUrl, @"^~/products/images/",
            "https://myleasingnunostorage.blob.core.windows.net/owners/");
    //    "https://supershopnunostorage.blob.core.windows.net/products/e1572b5b-3a31-4c9a-a68b-f13bc4f550d4";


    [Display(Name = "Image ID")] public Guid ImageId { get; set; }

    public string ImageFullIdUrl => ImageId == Guid.Empty
        ? "https://supershopweb.blob.core.windows.net/noimage/noimage.png"
        // : "https://supershopnunostorage.blob.core.windows.net/products/" +
        //   ImageId;
        : "https://supershopnunostorage.blob.core.windows.net/products/" +
          ImageId;
    //    "https://supershopnunostorage.blob.core.windows.net/products/
    //    e1572b5b-3a31-4c9a-a68b-f13bc4f550d4";


    [Display(Name = "Image GCP")] public Guid ImageIdGcp { get; set; }

    public string ImageFullIdGcpUrl => ImageId == Guid.Empty
        ? "https://supershopcet77.azurewebsites.net/images/noimage.png"
        : "https://storage.googleapis.com/supershoptpsicet77-jorge/products/" +
          ImageId;


    [Display(Name = "Image AWS")] public Guid ImageIdAws { get; set; }

    public string ImageFullIdAwsUrl => ImageId == Guid.Empty
        ? "https://supershopweb.blob.core.windows.net/noimage/noimage.png"
        : "https://supershopnunostorage.blob.core.windows.net/products/" +
          ImageId;
    //    "https://supershopnunostorage.blob.core.windows.net/products/e1572b5b-3a31-4c9a-a68b-f13bc4f550d4";


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


    [Required] public User User { get; set; }

    [Key] [Required] public int Id { get; set; }

    [Required] public bool WasDeleted { get; set; }
}