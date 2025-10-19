using LinkDev.Talabat.Core.Domain.Contracts.Persistence;
using LinkDev.Talabat.Core.Domain.Contracts.Persistence.DbInitialziers;
using LinkDev.Talabat.Infrastructure.Persistence._Identity;
using LinkDev.Talabat.Infrastructure.Persistence.Data;
using LinkDev.Talabat.Infrastructure.Persistence.Data.Interceptors;
using LinkDev.Talabat.Infrastructure.Persistence.Identity;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LinkDev.Talabat.Infrastructure.Persistence
{
    public static class DependencyInjection
    {

        // Extension method
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            #region Store Context
            //1. Registers StoreContext as a scoped service in the DI container
            //2. Configures it to use SQL Server with connection string from appsettings.json
            services.AddDbContext<StoreDbContext>((serviceProvider, optionBuilder) =>
            {
                optionBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(configuration.GetConnectionString("StoreContext"))
                .AddInterceptors(serviceProvider.GetRequiredService<AuditInterceptor>());
            });

            services.AddScoped<IStoreDbInitializer, StoreDbInitializer>();

            //services.AddScoped(typeof(ISaveChangesInterceptor), typeof(CutsomSaveChangesInterceptor));
            #endregion

            services.AddDbContext<StoreIdentityDbContext>((optionsBuilder) =>
            {
                optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(configuration.GetConnectionString("IdentityContext"));
            });

            services.AddScoped(typeof(IStoreIdentityDbInitializer), typeof(StoreIdentityDbInitializer));

            services.AddScoped(typeof(AuditInterceptor));

            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

            return services;
        }
    }
}
