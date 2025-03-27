using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace ProductAPI.Models {
    public class Review {
        [Key]
        public int Id {get; set;}

        [Required]
        [MaxLength(50)]
        public string? Title {get; set;}

        [Required]
        [MaxLength(500)]
        public string? Description {get; set;}

        [Required(ErrorMessage = "Set a rating for your review")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int? Rating {get; set;}

        public int ProductId {get; set;}

        [ForeignKey("ProductId")]
        [JsonIgnore]
        public Product? Product {get; set;}
    }
}
