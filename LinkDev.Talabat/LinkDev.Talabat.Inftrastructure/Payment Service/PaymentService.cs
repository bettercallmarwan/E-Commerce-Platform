using AutoMapper;
using LinkDev.Talabat.Shared.Models.Basket;
using LinkDev.Talabat.Core.Application.Exceptions;
using LinkDev.Talabat.Core.Domain.Contracts.Infrastructure;
using LinkDev.Talabat.Core.Domain.Contracts.Persistence;
using LinkDev.Talabat.Core.Domain.Entities.Basket;
using LinkDev.Talabat.Core.Domain.Entities.Orders;
using LinkDev.Talabat.Shared.Models;
using Microsoft.Extensions.Options;
using Stripe;
using Product = LinkDev.Talabat.Core.Domain.Entities.Products.Product;
using LinkDev.Talabat.Core.Domain.Specifications.Orders;
using Microsoft.Extensions.Logging;

namespace LinkDev.Talabat.Inftrastructure.PaymentService
{
    public class PaymentService(
        IBasketRepository basketRepository,
        IUnitOfWork unitOfWork,
        IOptions<RedisSettings> redisSettings,
        IOptions<StripeSettings> stripeSettings,
        ILogger<PaymentService> logger,
        IMapper mapper) : IPaymentService
    {
        private readonly RedisSettings _redisSettings = redisSettings.Value;
        private readonly StripeSettings _stripeSettings = stripeSettings.Value;

        public async Task<CustomerBasketDto> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _stripeSettings.Secretkey;

            var basket = await basketRepository.GetAsync(basketId);

            if (basket is null) throw new NotFoundException(nameof(CustomerBasket), basketId);

            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>().GetAsync(basket.DeliveryMethodId.Value);
                if (deliveryMethod is null) throw new NotFoundException(nameof(DeliveryMethod), basket.DeliveryMethodId.Value);

                basket.ShippingPrice = deliveryMethod.Cost;
            }

            if(basket.Items.Count() > 0)
            {
                var productRepo = unitOfWork.GetRepository<Product, int>();
                foreach (var item in basket.Items)
                {
                    var product = await productRepo.GetAsync(item.Id);
                    if (product is null) throw new NotFoundException(nameof(Product), item.Id);

                    if (item.Price != product.Price)
                    {
                        item.Price = product.Price; 
                    }
                }
            }

            PaymentIntent? paymentIntent = null;
            PaymentIntentService paymentIntentService = new PaymentIntentService();

            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Price * 100 * item.Quantity) + (long)basket.ShippingPrice * 100,
                    Currency = "USD",
                    PaymentMethodTypes = new List<string>() { "card"}
                };

                paymentIntent = await paymentIntentService.CreateAsync(options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Price * 100 * item.Quantity) + (long)basket.ShippingPrice * 100
                };

                await paymentIntentService.UpdateAsync(basket.PaymentIntentId, options);
            }

            await basketRepository.UpdateAsync(basket, TimeSpan.FromDays(_redisSettings.TimeToLiveInDays));

            return mapper.Map<CustomerBasketDto>(basket);
        }

        public async Task UpdateOrderPaymentStatus(string requestBody, string header)
        {
            var stripeEvent = EventUtility.ConstructEvent(requestBody, header, _stripeSettings.WebhookSecret);

            var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;

            Order? order;
            if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
            {
                order = await UpdatePaymentIntent(paymentIntent.Id, true);
                logger.LogInformation($"Order succeeded with payment intent Id : {paymentIntent.Id}");
            }

            else if (stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
            {
                order = await UpdatePaymentIntent(paymentIntent.Id, false);
                logger.LogInformation($"Order failed with payment intent Id : {paymentIntent.Id}");
            }
        }

        private async Task<Order> UpdatePaymentIntent(string paymentIntentId, bool isPaid)
        {
            var orderRepo = unitOfWork.GetRepository<Order, int>();

            var spec = new OrderByPaymentIntentSpecifications(paymentIntentId);

            var order = await orderRepo.GetWithSpecAsync(spec);

            if (order is null) throw new NotFoundException(nameof(Order), $"paymentIntentId: {paymentIntentId}");

            if (isPaid)
                order.Status = OrderStatus.PayementRecieved;
            else
                order.Status = OrderStatus.PayementFailed;

            orderRepo.Update(order);

            await unitOfWork.CompleteAsync();

            return order;
        }
    }
}
