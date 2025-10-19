using LinkDev.Talabat.APIs.Controllers.Base;
using LinkDev.Talabat.Core.Application.Abstraction.Services;
using LinkDev.Talabat.Shared.Models.Basket;
using Microsoft.AspNetCore.Mvc;

namespace LinkDev.Talabat.APIs.Controllers.Controllers.Basket
{
    public class BasketController(IServiceManager serviceManager) : BaseApiController
    {
        [HttpGet] // GET: /api/Basket?id=1
        public async Task<ActionResult<CustomerBasketDto>> GetBasket(string id)
        {
            var basket = await serviceManager.BasketService.GetCustomerBasketAsync(id);
            return Ok(basket);

        }

        [HttpPost] // GET: /api/Basket
        public async Task<ActionResult<CustomerBasketDto>> UpdateBasket(CustomerBasketDto basketDto)
        {
            var basket = await serviceManager.BasketService.UpdateCustomerBasketAsync(basketDto);
            return Ok(basket);
        }

        [HttpDelete]
        public async Task DeleteBasket(string id)
        {
            await serviceManager.BasketService.DeleteCustomerBasketAsync(id);
        }
    }
}
