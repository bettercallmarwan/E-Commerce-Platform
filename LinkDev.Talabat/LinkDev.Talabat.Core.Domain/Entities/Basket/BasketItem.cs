namespace LinkDev.Talabat.Core.Domain.Entities.Basket
{
    public class BasketItem
    {
        public int Id { get; set; } // Product Id (same id as basket item)
        public required string ProductName { get; set; } // to prevent new() from initializing ProductName to null
        public string? PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? Brand { get; set; } // name
        public string? Category { get; set; } // name
    }
}
 