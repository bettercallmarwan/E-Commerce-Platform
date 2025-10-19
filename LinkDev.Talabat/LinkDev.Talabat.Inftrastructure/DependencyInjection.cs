using LinkDev.Talabat.Core.Domain.Contracts.Infrastructure;
using LinkDev.Talabat.Inftrastructure.Basket_Repository;
using LinkDev.Talabat.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using _PaymentService = LinkDev.Talabat.Inftrastructure.PaymentService.PaymentService;

namespace LinkDev.Talabat.Inftrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastrcutureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(typeof(IConnectionMultiplexer), (serviceProvider) =>
            {
                var connectionString = configuration.GetConnectionString("Redis");
                var connectionMultiplexerObj = ConnectionMultiplexer.Connect(connectionString!);
                return connectionMultiplexerObj;
            });

            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));

            services.Configure<RedisSettings>(configuration.GetSection("RedisSettings"));

            services.Configure<StripeSettings>(configuration.GetSection("StripeSettings"));

            services.AddScoped(typeof(IPaymentService), typeof(_PaymentService));

            return services;
        }
    }
}
