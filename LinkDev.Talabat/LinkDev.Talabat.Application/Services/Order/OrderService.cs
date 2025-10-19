using LinkDev.Talabat.Core.Application.Abstraction.Models.Orders;
using LinkDev.Talabat.Core.Application.Abstraction.Services.Basket;
using LinkDev.Talabat.Core.Domain.Contracts.Persistence;
using LinkDev.Talabat.Core.Domain.Entities.Orders;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using LinkDev.Talabat.Core.Application.Abstraction.Services.Orders;
using AutoMapper;
using LinkDev.Talabat.Core.Application.Exceptions;
using LinkDev.Talabat.Core.Domain.Specifications.Orders;
using LinkDev.Talabat.Core.Domain.Contracts.Infrastructure;

namespace LinkDev.Talabat.Core.Application.Services.Order
{
    internal class OrderService(IBasketService basketService, IUnitOfWork unitOfWork, IMapper mapper, IPaymentService paymentService) : IOrderService
    {
        public async Task<OrderToReturnDto> CreateOrderAync(string buyerEmail, OrderToCreateDto order)
        {
            var basket = await basketService.GetCustomerBasketAsync(order.BasketId);

            // 1. get selected items in basket from products repo
            var orderItems = new List<OrderItem>();
            if (basket.Items.Count() > 0)
            {
                var productRepo = unitOfWork.GetRepository<Product, int>();
                foreach (var item in basket.Items)
                {
                    var product = await productRepo.GetAsync(item.Id);

                    if (product is not null)
                    {
                        var productItemOrdered = new ProductItemOrdered()
                        {
                            ProductId = product.Id,
                            ProductName = product.Name,
                            PictureUrl = product.PictureUrl ?? ""
                        };

                        var orderItem = new OrderItem()
                        {
                            product = productItemOrdered,
                            Price = product.Price,
                            Quantity = item.Quantity
                        };

                        orderItems.Add(orderItem);
                    }
                }
            }

            // 2. calculate subtotal

            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);

            // 3.map address

            var address = mapper.Map<Address>(order.ShippingAddress);

            var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>().GetAsync(order.DeliveryMethodId);

            // 4. create order

            var orderRepo = unitOfWork.GetRepository<Domain.Entities.Orders.Order, int>();

            var orderSpecs = new OrderByPaymentIntentSpecifications(basket.PaymentIntentId!);

            var existingOrder = await orderRepo.GetWithSpecAsync(orderSpecs);

            if (existingOrder is not null)
            {
                // Delete OrderItems first to avoid foreign key constraint violation
                var orderItemRepo = unitOfWork.GetRepository<OrderItem, int>();
                foreach (var item in existingOrder.Items)
                {
                    orderItemRepo.Delete(item);
                }

                orderRepo.Delete(existingOrder);
                await paymentService.CreateOrUpdatePaymentIntent(basket.Id);
            }

            var orderToCreate = new Domain.Entities.Orders.Order()
            {
                BuyerEmail = buyerEmail,
                ShippingAddress = address,
                DeliveryMethod = deliveryMethod,
                Items = orderItems,
                SubTotal = subTotal,
                PaymentIntentId = basket.PaymentIntentId!
            };

            await orderRepo.AddAsync(orderToCreate);

            var created = await unitOfWork.CompleteAsync() > 0;

            if (!created) throw new BadRequestException("an error has been occured during creating order");

            return mapper.Map<OrderToReturnDto>(orderToCreate);
        }

        public async Task<IEnumerable<OrderToReturnDto>> GetOrdersForUserAsync(string buyerEmail)
        {
            var orderSpecs = new OrderSpecifications(buyerEmail);

            var orders = await unitOfWork.GetRepository<Domain.Entities.Orders.Order, int>().GetAllWithSpecAsync(orderSpecs);

            return mapper.Map<IEnumerable<OrderToReturnDto>>(orders);
        }
        public async Task<OrderToReturnDto> GetOrderByIdAsync(string buyerEmail, int orderId)
        {
            var orderSpecs = new OrderSpecifications(buyerEmail, orderId);

            var order = await unitOfWork.GetRepository<Domain.Entities.Orders.Order, int>().GetWithSpecAsync(orderSpecs);
            if (order is null) throw new NotFoundException(nameof(order), orderId);

            return mapper.Map<OrderToReturnDto>(order);
        }
        public async Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = await unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync();
            return mapper.Map<IEnumerable<DeliveryMethodDto>>(deliveryMethods);
        }
    }
}