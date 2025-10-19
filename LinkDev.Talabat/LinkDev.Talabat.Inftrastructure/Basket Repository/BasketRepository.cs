using LinkDev.Talabat.Core.Domain.Contracts.Infrastructure;
using LinkDev.Talabat.Core.Domain.Entities.Basket;
using StackExchange.Redis;
using System.Text.Json;

namespace LinkDev.Talabat.Inftrastructure.Basket_Repository
{
    internal class BasketRepository : IBasketRepository  
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task<CustomerBasket?> GetAsync(string id)
        {
            var basket = await _database.StringGetAsync(id);

            return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(basket!);
        }

        public async Task<CustomerBasket?> UpdateAsync(CustomerBasket basket, TimeSpan timeToLive)
        {
            var value = JsonSerializer.Serialize(basket); // because StringSetAsync only takes json as value

            var updated = await _database.StringSetAsync(basket.Id, value, timeToLive);

            return updated ? basket : null;
        }
         
        public async Task<bool> DeleteAsync(string id) => await _database.KeyDeleteAsync(id);
    }
}
