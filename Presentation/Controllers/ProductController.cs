using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared;
using Shared.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class ProductController(IServiceManager serviceManager) : ApiController
    {
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<ProductResultDto>>> GetAllProductsAsync([FromQuery]ProductSpecificationParams specifications) 
        {
            var products = await serviceManager.productService.GetAllProductsAsync(specifications);
            return Ok(products);
        }

        [HttpGet]
        public async Task<ActionResult<ProductResultDto>> GetProductBtIdAsync(int id)
        {
            var product = await serviceManager.productService.GetProductByIdAsync(id);
            return Ok(product);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BrandResultDto>>> GetAllBrandsAsync()
        {
            var Brands = await serviceManager.productService.GetAllBrandsAsync();
            return Ok(Brands);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeResultDto>>> GetAllTypesAsync()
        {
            var types = await serviceManager.productService.GetAllTypesAsync();
            return Ok(types);
        }
    }
}
