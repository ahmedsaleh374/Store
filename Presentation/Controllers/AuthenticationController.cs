using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.IdentityDtos;
using Shared.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ProducesResponseType(typeof(UserResultDto), (int)HttpStatusCode.OK)]
    public class AuthenticationController(IServiceManager serviceManager) :ApiController
    {
        [HttpPost]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            return Ok(await serviceManager.authenticationService.LoginAsync(loginDto));
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            return Ok(await serviceManager.authenticationService.RegisterAsync(registerDto));
        }
    }
}
