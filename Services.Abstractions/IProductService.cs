using Shared.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResultDto>> GetAllProductsAsync();

        Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync();

        Task<IEnumerable<TypeResultDto>> GetAllTypesAsync();

        Task<ProductResultDto> GetProductByIdAsync(int id);
    }
}
