using LinkDev.Talabat.Core.Application.Abstraction.Models.Auth;
using LinkDev.Talabat.Core.Application.Abstraction.Models.Common;
using System.Security.Claims;

namespace LinkDev.Talabat.Core.Application.Abstraction.Services.Auth
{
    public interface IAuthService
    {
        Task<UserDto> LoginAsync(LoginDto model);
        Task<UserDto> RegisterAysnc(RegisterDto model);
        Task<UserDto> GetCurrentUser(ClaimsPrincipal claimsPrincipal);
        Task<AddressDto?> GetUserAddress(ClaimsPrincipal claimsPrincipal);
        Task<AddressDto> UpdateUserAddress(ClaimsPrincipal claimsPrincipal, AddressDto addressDto);
        Task<bool> EmailExists(string email);
    }
}
