using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace ProductAPI.Models {
    public class Wishlist {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price {get; set;}

        [Required]
        public int Stock {get; set;}

        [Required]
        public string Size {get; set;}

        public string? ImageUrl {get; set;}
        public string? ImageFileName {get; set;}

        public int? UserId {get; set;} 

        [ForeignKey("ProductId")]
        [JsonIgnore]
        public Product? Product {get; set;}
    }
}