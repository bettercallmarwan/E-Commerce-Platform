using System.ComponentModel.DataAnnotations;

namespace LinkDev.Talabat.Shared.Models.Basket
{
    public class BasketItemDto
    {
        [Required]
        public int Id { get; set; } // Product Id (same id as basket item)
        [Required]
        public required string ProductName { get; set; } // to prevent new() fr0om initializing ProductName to null
        public string? PictureUrl { get; set; }
        [Required]
        [Range(.1, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage ="Quantity must be at least one item.")]
        public int Quantity { get; set; }
        public string? Brand { get; set; } // name
        public string? Category { get; set; } // name
    }
}
