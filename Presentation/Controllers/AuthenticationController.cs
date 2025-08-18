using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.IdentityDtos;
using Shared.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ProducesResponseType(typeof(UserResultDto), (int)HttpStatusCode.OK)]
    public class AuthenticationController(IServiceManager serviceManager) : ApiController
    {
        [HttpPost]
        public async Task<ActionResult<UserResultDto>> Login(LoginDto loginDto)
        {
            return Ok(await serviceManager.authenticationService.LoginAsync(loginDto));
        }

        [HttpPost]
        public async Task<ActionResult<UserResultDto>> Register(RegisterDto registerDto)
        {
            return Ok(await serviceManager.authenticationService.RegisterAsync(registerDto));
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserResultDto>> GetCurrentUser()
        {
            var email = User.FindFirst(ClaimTypes.Email).Value;
            var result = await serviceManager.authenticationService.GetUserByEmailAsync(email);
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<bool>> IsEmailExists()
        {
            var email = User.FindFirst(ClaimTypes.Email).Value;
            var result = await serviceManager.authenticationService.IsEmailExists(email);
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var email = User.FindFirst(ClaimTypes.Email).Value;
            var result = await serviceManager.authenticationService.GetUserAddressAsync(email);
            return Ok(result);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto addressDto)
        {
            var email = User.FindFirst(ClaimTypes.Email).Value;
            var result = await serviceManager.authenticationService.UpdateUserAddressAsync(email, addressDto);
            return Ok(result);
        }
    }
}
