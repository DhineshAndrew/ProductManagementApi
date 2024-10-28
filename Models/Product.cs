using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManagementApi.Models
{
    public class Product
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        [MaxLength(100)]
        public string productName { get; set; }

        [Required]
        [MaxLength(500)]
        public string description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal price { get; set; }

        [Required]
        public string category { get; set; }

        public string ImagePath { get; set; } 
    }
}
