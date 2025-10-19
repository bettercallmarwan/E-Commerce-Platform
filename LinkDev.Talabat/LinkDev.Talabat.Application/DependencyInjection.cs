using LinkDev.Talabat.Core.Application.Abstraction.Services;
using LinkDev.Talabat.Core.Application.Abstraction.Services.Basket;
using LinkDev.Talabat.Core.Application.Abstraction.Services.Orders;
using LinkDev.Talabat.Core.Application.Mapping;
using LinkDev.Talabat.Core.Application.Services;
using LinkDev.Talabat.Core.Application.Services.Basket;
using LinkDev.Talabat.Core.Application.Services.Order;
using Microsoft.Extensions.DependencyInjection;

namespace LinkDev.Talabat.Core.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(config => config.AddProfile<MappingProfile>());
            
            services.AddScoped<ProductPictureUrlResolver>();

            services.AddScoped<OrderItemPictureUrlResolver>();


            // Register BasketService
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped(typeof(Func<IBasketService>), (serviceProvider) =>
            {
                return () => serviceProvider.GetRequiredService<IBasketService>();
            });

            
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped(typeof(Func<IOrderService>), (ServiceProvider) =>
            {
                return () => ServiceProvider.GetRequiredService<IOrderService>();
            });

            services.AddScoped<IServiceManager, ServiceManager>();
            return services;
        }
    }
}
