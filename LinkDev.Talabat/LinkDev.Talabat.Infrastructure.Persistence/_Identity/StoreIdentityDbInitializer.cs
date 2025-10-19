using LinkDev.Talabat.Core.Domain.Contracts.Persistence.DbInitialziers;
using LinkDev.Talabat.Core.Domain.Entities.Identity;
using LinkDev.Talabat.Infrastructure.Persistence.Common;
using LinkDev.Talabat.Infrastructure.Persistence.Identity;
using Microsoft.AspNetCore.Identity;

namespace LinkDev.Talabat.Infrastructure.Persistence._Identity
{
    internal class StoreIdentityDbInitializer(StoreIdentityDbContext _dbContext, UserManager<ApplicationUser> _userManager) 
        : DbInitializer(_dbContext), IStoreIdentityDbInitializer
    {
        public override async Task SeedAsync()
        {
                var user = new ApplicationUser()
                { 
                    DisplayName = "Marwan Osama",
                    UserName = "marwan.osama",
                    Email = "marwan@gmail.com",
                    PhoneNumber = "12345678",
                };

                await _userManager.CreateAsync(user, "P@sswOrd"); 
        }
    }
}
//Password1_