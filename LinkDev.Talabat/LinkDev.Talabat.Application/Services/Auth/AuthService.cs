using AutoMapper;
using LinkDev.Talabat.Core.Application.Abstraction.Models.Auth;
using LinkDev.Talabat.Core.Application.Abstraction.Models.Common;
using LinkDev.Talabat.Core.Application.Abstraction.Services.Auth;
using LinkDev.Talabat.Core.Application.Exceptions;
using LinkDev.Talabat.Core.Application.Extensions;
using LinkDev.Talabat.Core.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LinkDev.Talabat.Core.Application.Services.Auth
{
    public class AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<JwtSettings> jwtSettings, IMapper mapper) : IAuthService
    // the SignInManager is from the jwt bearer package and not from the identity package
    {
        private readonly JwtSettings _jwtSettings = jwtSettings.Value;

        public async Task<bool> EmailExists(string email)
        {
            return await userManager.FindByEmailAsync(email!) is not null;
        }

        public async Task<UserDto> GetCurrentUser(ClaimsPrincipal claimsPrincipal)
        {
            var email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);

            var user = await userManager.FindByEmailAsync(email!);

            return new UserDto()
            {
                Id = user!.Id!,
                DisplayName = user.DisplayName,
                Email = user.Email!,
                Token = await GenerateTokenAysnc(user)
            };
        }

        public async Task<AddressDto?> GetUserAddress(ClaimsPrincipal claimsPrincipal)
        {
            var user = await userManager.FindUserWithAddress(claimsPrincipal);

            var address = mapper.Map<AddressDto>(user!.Address);

            return address; 
        }

        public async Task<UserDto> LoginAsync(LoginDto model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);

            if (user is null) throw new UnAuthorizedException("Invalid Login");

            var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: true);

            if (result.IsNotAllowed) throw new UnAuthorizedException("Account not confirmed yet.");

            if(result.IsLockedOut) throw new UnAuthorizedException("Account is locked, Please try again later");

            //if(result.RequiresTwoFactor)
            if (!result.Succeeded) throw new UnAuthorizedException("Invalid Login.");

            var response = new UserDto()
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email!,
                Token = await GenerateTokenAysnc(user)
            };

            return response;
        }

        public async Task<UserDto> RegisterAysnc(RegisterDto model)
        {
            var user = new ApplicationUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.UserName,
                PhoneNumber = model.PhoneNumber
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) throw new ValidationException() { Errors = result.Errors.Select(E => E.Description) };

            var response = new UserDto()
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email!,
                Token = await GenerateTokenAysnc(user)
            };

            return response;
        }

        public async Task<AddressDto> UpdateUserAddress(ClaimsPrincipal claimsPrincipal, AddressDto addressDto)
        {
            var updatedAddress = mapper.Map<Address>(addressDto);

            var user = await userManager.FindUserWithAddress(claimsPrincipal!);

            if (user?.Address is not null)
                updatedAddress.Id = user.Address.Id;

            user.Address = updatedAddress;

            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded) throw new BadRequestException(result.Errors.Select(error => error.Description).Aggregate((x, y) => $"{x}, {y}"));

            return addressDto;
        }

        private async Task<string> GenerateTokenAysnc(ApplicationUser user)
        {
            var userClaims = await userManager.GetClaimsAsync(user); 

            // user roles to put as claims in jwt
            var rolesAsClaims = new List<Claim>();
            var roles = await userManager.GetRolesAsync(user);
            foreach(var role in roles)
                rolesAsClaims.Add(new Claim(ClaimTypes.Role, role.ToString()));

            var Claims = new List<Claim>()
            {
                new Claim(ClaimTypes.PrimarySid, user.Id),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.GivenName, user.DisplayName),
            }
            .Union(userClaims)
            .Union(rolesAsClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var tokenObj = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                claims: Claims,
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenObj);
        }
    }
}
