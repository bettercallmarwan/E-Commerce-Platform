using LinkDev.Talabat.Core.Domain.Entities.Basket;

namespace LinkDev.Talabat.Core.Domain.Contracts.Infrastructure
{
    public interface IBasketRepository
    {
        Task<CustomerBasket?> GetAsync(string id);
        Task<CustomerBasket?> UpdateAsync(CustomerBasket basket, TimeSpan timeToLive);
        Task<bool> DeleteAsync(string id);
    }
}
