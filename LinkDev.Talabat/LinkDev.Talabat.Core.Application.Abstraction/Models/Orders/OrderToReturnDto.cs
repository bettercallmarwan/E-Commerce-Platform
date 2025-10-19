using LinkDev.Talabat.Core.Application.Abstraction.Models.Common;
using LinkDev.Talabat.Core.Domain.Entities.Orders;

namespace LinkDev.Talabat.Core.Application.Abstraction.Models.Orders
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }
        public required string BuyerEmail { get; set; }
        public DateTime OrderDate { get; set; }
        public required string Status { get; set; }
        public virtual AddressDto ShippingAddress { get; set; }

        public int? DeliveryMethodId { get; set; }
        public string? DeliveryMethod { get; set; }
        public virtual required ICollection<OrderItemDto> Items { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public string PaymentIntentId { get; set; }
    }
}
