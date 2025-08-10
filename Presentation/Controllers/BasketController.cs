using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.BasketDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[Controller]/[Action]")]
    public class BasketController(IServiceManager serviceManager) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<BasketDto>> GetAsync(String id)
        {
            return Ok(await serviceManager.BasketService.GetBasketAsync(id));
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(String id)
        {
            await serviceManager.BasketService.DeleteBasketAsync(id);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<BasketDto>> UpdateAsync(BasketDto basketDto)
        {
            var basket = await serviceManager.BasketService.UpdateBasketAsync(basketDto);
            return Ok(basket);
            //return RedirectToAction(nameof(GetAsync));
        }
    }
}
