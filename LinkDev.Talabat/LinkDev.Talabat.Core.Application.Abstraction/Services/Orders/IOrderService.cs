using LinkDev.Talabat.Core.Application.Abstraction.Models.Orders;

namespace LinkDev.Talabat.Core.Application.Abstraction.Services.Orders
{
    public interface IOrderService
    {
        Task<OrderToReturnDto> CreateOrderAync(string buyerEmail, OrderToCreateDto order);
        Task<OrderToReturnDto> GetOrderByIdAsync(string buyerEmail, int orderId);
        Task<IEnumerable<OrderToReturnDto>> GetOrdersForUserAsync(string buyerEmail);
        Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodsAsync();
    }
}
