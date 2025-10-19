using LinkDev.Talabat.Core.Application.Abstraction.Models.Auth;
using LinkDev.Talabat.Core.Application.Abstraction.Services.Auth;
using LinkDev.Talabat.Core.Application.Services.Auth;
using LinkDev.Talabat.Core.Domain.Entities.Identity;
using LinkDev.Talabat.Infrastructure.Persistence.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;

namespace LinkDev.Talabat.APIs.Extensions
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            // configured JwtSettings, so when JwtSettings is requested from the jwt container
            // it's properties are initialized with values from jwtSettings, which is in appsettings
            services.Configure<JwtSettings>(configuration.GetSection("jwtSettings"));

            services.AddIdentity<ApplicationUser, IdentityRole>(identityOptions =>
            {
                identityOptions.User.RequireUniqueEmail = true;
                //identityOptions.User.AllowedUserNameCharacters = "asghaguS";

                identityOptions.SignIn.RequireConfirmedAccount = true;
                identityOptions.SignIn.RequireConfirmedEmail = true;
                identityOptions.SignIn.RequireConfirmedPhoneNumber = true;

                //identityOptions.Password.RequireNonAlphanumeric = true;
                //identityOptions.Password.RequiredUniqueChars = 2;
                //identityOptions.Password.RequiredLength = 6;
                //identityOptions.Password.RequireDigit = true;
                //identityOptions.Password.RequireLowercase = true;
                //identityOptions.Password.RequireUppercase = true;

                identityOptions.Lockout.AllowedForNewUsers = true;
                identityOptions.Lockout.MaxFailedAccessAttempts = 10;
                identityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);


            })
                .AddEntityFrameworkStores<StoreIdentityDbContext>();

            services.AddScoped(typeof(IAuthService), typeof(AuthService));
            services.AddScoped(typeof(Func<IAuthService>), (serviceProvider) =>
            {
                return () => serviceProvider.GetService<IAuthService>();
            });


            // called by default when calling (AddIdentity)
            services.AddAuthentication((authenticationOptions) =>
            {
                authenticationOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer((configurationOptions) =>
                {
                    configurationOptions.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,

                        ClockSkew = TimeSpan.FromMinutes(0),
                        ValidIssuer = configuration["jwtSettings:Issuer"],
                        ValidAudience = configuration["jwtSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwtSettings:Key"]!)),


                    };
                });

            return services;
        }
    }
}
