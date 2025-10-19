using LinkDev.Talabat.APIs.Controllers.Base;
using LinkDev.Talabat.Core.Application.Abstraction.Models.Orders;
using LinkDev.Talabat.Core.Application.Abstraction.Services;
using LinkDev.Talabat.Core.Domain.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LinkDev.Talabat.APIs.Controllers.Controllers.Order
{
    //[Authorize]
    public class OrdersController(IServiceManager serviceManager) : BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderToCreateDto orderDto)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

            var result = await serviceManager.OrderService.CreateOrderAync(buyerEmail!, orderDto);

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderToReturnDto>>> GetOrdersForUser()
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

            var result = await serviceManager.OrderService.GetOrdersForUserAsync(buyerEmail!);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrder(int id)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

            var result = await serviceManager.OrderService.GetOrderByIdAsync(buyerEmail ,id);

            return Ok(result);
        }

        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IEnumerable<DeliveryMethodDto>>> GetDeliveryMethods()
        {
            var result = await serviceManager.OrderService.GetDeliveryMethodsAsync();
            return Ok(result);
        }
    }
}
