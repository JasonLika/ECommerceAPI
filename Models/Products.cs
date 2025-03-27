using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace ProductAPI.Models
{
    public class Product {
        [Key]
        public int Id {get; set;}

        [Required]
        [MaxLength(50)]
        public string? Name {get; set;}

        [Required]
        [MaxLength(500)]
        public string? Description {get; set;}

        [Required]
        public decimal Price {get; set;}

        [Required]
        public int Stock {get; set;}

        [Required]
        public string? Size {get; set;}

        public string? ImageUrl {get; set;}
        public string? ImageFileName {get; set;}
        
        public ICollection<Review> Reviews {get; set;} = new List<Review>();
    }
}