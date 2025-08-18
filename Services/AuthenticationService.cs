using AutoMapper;
using Domain.Entities.Identity;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Abstractions;
using Shared.IdentityDtos;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AuthenticationService(UserManager<User> userManager
        , IMapper mapper
        , ILogger<AuthenticationService> logger
        /*,IConfiguration configuration*/
        , IOptions<JwtOptions> options) : IAuthenticationService
    {

        public async Task<UserResultDto> LoginAsync(LoginDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if (user is null)
            {
                throw new UnauthorizedAccessException($"Email : {loginDto.Email} is not exist !");
            }
            var result = await userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result)
                throw new UnauthorizedAccessException();

            return new UserResultDto
                 (
                    user.DisplayName,
                    user.Email!,
                    await CreateTokenAsync(user)

                 );
        }

        public async Task<UserResultDto> RegisterAsync(RegisterDto registerDto)
        {
            var user = new User
            {
                DisplayName = registerDto.DisplayName,
                UserName = registerDto.UserName,
                PhoneNumber = registerDto.PhoneNumber,
                Email = registerDto.Email,
            };
            var result = await userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    logger.LogError(item.Description);
                }
                var errors = result.Errors.Select(x => x.Description).ToList();
                throw new ValidationException(errors);
            }

            return new UserResultDto
                (
                    user.DisplayName,
                    user.Email,
                    await CreateTokenAsync(user)
                );
        }

        private async Task<string> CreateTokenAsync(User user)
        {
            //MAPPING FROM APP SETTING 
            var JwtOptions = options.Value;

            //payload 
            //1 - create claims 
            //var userClaims = await userManager.GetClaimsAsync(user);
            //var claims = new List<Claim>();
            //if (userClaims is null)
            //{
            //    claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            //    claims.Add(new Claim(ClaimTypes.Email, user.Email));
            //}


            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()));

            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            //signature 
            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JwtOptions[]"))); From configurations directly
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.SecurityKey));
            //header
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //toke
            var token = new JwtSecurityToken
                (
                    claims: claims,
                    signingCredentials: creds,
                    expires: DateTime.UtcNow.AddDays(JwtOptions.DurationInDays),
                    issuer: JwtOptions.Issuer,
                    audience: JwtOptions.Audience
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task<UserResultDto> GetUserByEmailAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new UserNotFoundException(email);
            }

            var userDto = new UserResultDto
                (
                   user.DisplayName,
                   user.Email,
                   await CreateTokenAsync(user)
                );
            return userDto;
        }

        public async Task<bool> IsEmailExists(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new UserNotFoundException(email);
            }

            return user.Email != null;
        }

        public async Task<AddressDto> GetUserAddressAsync(string email)
        {
            var user = await userManager.Users.Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user is null)
            {
                throw new UserNotFoundException(email);
            }

            var addressDto = mapper.Map<AddressDto>(user.Address);
            return addressDto;
        }

        public async Task<AddressDto> UpdateUserAddressAsync(string email, AddressDto addressDto)
        {
            var user = await userManager.Users.Include(u => u.Address)
                 .FirstOrDefaultAsync(u => u.Email == email);

            if (user is null)
            {
                throw new UserNotFoundException(email);
            }

            var mappedAddress = mapper.Map<Address>(addressDto);
            user.Address = mappedAddress;

            await userManager.UpdateAsync(user);

            return addressDto;
        }


    }
}
