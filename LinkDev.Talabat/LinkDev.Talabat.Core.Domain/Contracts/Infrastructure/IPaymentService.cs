

using LinkDev.Talabat.Shared.Models.Basket;

namespace LinkDev.Talabat.Core.Domain.Contracts.Infrastructure
{
    public interface IPaymentService
    {
        Task<CustomerBasketDto> CreateOrUpdatePaymentIntent(string basketId);
        Task UpdateOrderPaymentStatus(string requestBody, string header);
    }
}
