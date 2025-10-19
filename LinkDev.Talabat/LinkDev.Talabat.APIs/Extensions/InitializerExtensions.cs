using LinkDev.Talabat.Core.Domain.Contracts.Persistence.DbInitialziers;

namespace LinkDev.Talabat.APIs.Extensions
{
    public static class InitializerExtensions
    {
        public static async Task<WebApplication> InitializeDbAsync(this WebApplication webApplication)
        {
            using var scope = webApplication.Services.CreateAsyncScope(); // 1. Creating a scope to get scoped services
            var services = scope.ServiceProvider; // 2.The ServiceProvider is like a "factory" or "warehouse" that knows how to create and deliver the services you've registered.

            /* This method:

            1.Looks up the registration for StoreContext
            2.Creates dependencies(connection strings, options, etc.)
            3.Instantiates the StoreContext
            4.Returns the configured instance
            5.Throws exception if the service isn't registered (that's why it's "Required")*/
            var storeContextInitializer = services.GetRequiredService<IStoreDbInitializer>();
            var identityContextInitializer = services.GetRequiredService<IStoreIdentityDbInitializer>();



            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                await storeContextInitializer.InitializeAsync();
                await storeContextInitializer.SeedAsync();

                await identityContextInitializer.InitializeAsync();
                await identityContextInitializer.SeedAsync();
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "an error has been occured during applying migrations or data seeding");
            }

            return webApplication;
        }
    }
}
