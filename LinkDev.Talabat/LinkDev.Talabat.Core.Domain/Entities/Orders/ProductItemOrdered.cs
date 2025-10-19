using Microsoft.EntityFrameworkCore;

namespace LinkDev.Talabat.Core.Domain.Entities.Orders
{
    [Owned]
    public class ProductItemOrdered
    {
        public ProductItemOrdered()
        {
            
        }
        public int ProductId { get; set; }
        public required string ProductName { get; set; }
        public required string PictureUrl { get; set; }
    }
}
