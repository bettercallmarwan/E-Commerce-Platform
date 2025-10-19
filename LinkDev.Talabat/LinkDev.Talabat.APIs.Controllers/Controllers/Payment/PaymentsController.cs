using LinkDev.Talabat.APIs.Controllers.Base;
using LinkDev.Talabat.Shared.Models.Basket;
using LinkDev.Talabat.Core.Domain.Contracts.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace LinkDev.Talabat.APIs.Controllers.Controllers.Payment
{
    public class PaymentsController(IPaymentService paymentService) : BaseApiController
    {
        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var result = await paymentService.CreateOrUpdatePaymentIntent(basketId);
            return Ok(result);
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> WebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            await paymentService.UpdateOrderPaymentStatus(json, Request.Headers["Stripe-Signature"]!);

            return Ok();
        }
    }
}
