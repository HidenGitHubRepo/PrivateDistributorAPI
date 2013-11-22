using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using CodeFirst.Model;

namespace PrivateDistributor.Services.Models
{
    public class ProductRequestModel
    {
        public static Func<ProductRequestModel, Product > FromEntity { get; set; }

        [DataMember(Name = "SessionKey")]
        public string SessionKey { get; set; }

        [DataMember(Name = "PublicId")]
        public string PublicId { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "NutritiveValue")]
        public string NutritiveValue { get; set; }

        [DataMember(Name = "Description")]
        public string Description { get; set; }

        [DataMember(Name = "Storing")]
        public string Storing { get; set; }

        [DataMember(Name = "Price")]
        public string Price { get; set; }

        [DataMember(Name = "ImageUrl")]
        public string ImageUrl { get; set; }

        [DataMember(Name = "Brand")]
        public string Brand { get; set; }

        [DataMember(Name = "MadeIn")]
        public string MadeIn { get; set; }

        [DataMember(Name = "Category")]
        public string Category { get; set; }

        static ProductRequestModel()
        {
            FromEntity = x => new Product
            {
                PublicId = x.PublicId,
                Name = x.Name,
                NutritiveValue = x.NutritiveValue,
                Description = x.Description,
                Storing = x.Storing,
                ImageUrl = x.ImageUrl,
                Brand = x.Brand,
                MadeIn = x.MadeIn,
                Category = x.Category
            };
        }
    }

    [DataContract]
    public class ProductResponseModel
    {
        [DataMember(Name = "Id")]
        public int Id;

        public static Func<Product, ProductResponseModel> FromEntity { get; set; }

        [DataMember(Name = "ImageType")]
        public string ImageType;

        [DataMember(Name = "PublicId")]
        public string PublicId { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "NutritiveValue")]
        public string NutritiveValue { get; set; }

        [DataMember(Name = "Description")]
        public string Description { get; set; }

        [DataMember(Name = "Storing")]
        public string Storing { get; set; }

        [DataMember(Name = "Price")]
        public decimal Price { get; set; }

        [DataMember(Name = "ImageUrl")]
        public string ImageUrl { get; set; }

        [DataMember(Name = "Brand")]
        public string Brand { get; set; }

        [DataMember(Name = "MadeIn")]
        public string MadeIn { get; set; }

        [DataMember(Name = "Category")]
        public string Category { get; set; }

        static ProductResponseModel()
        {
            FromEntity = x => new ProductResponseModel
            {
                Id = x.Id,
                PublicId = x.PublicId,
                Name = x.Name,
                NutritiveValue = x.NutritiveValue,
                Description = x.Description,
                Storing = x.Storing,
                Price = x.Price,
                ImageType = x.ImageType,
                ImageUrl = x.ImageUrl,
                Brand = x.Brand,
                MadeIn = x.MadeIn,
                Category = x.Category
            };
        }
    }
}