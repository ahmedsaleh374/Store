using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Services.Abstractions;
using Services.Specifications;
using Shared;
using Shared.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ProductService(IUnitOfWork unitOfWork,IMapper mapper) : IProductService
    {
        public async Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync()
        {
            var Brands = await unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
            var mappedBrands = mapper.Map< IEnumerable<BrandResultDto>>(Brands);
            return mappedBrands;
        }

        public async Task<IEnumerable<TypeResultDto>> GetAllTypesAsync()
        {
            var Types = await unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            var mappedTypes = mapper.Map<IEnumerable<TypeResultDto>>(Types);
            return mappedTypes;
        }

        public async Task<PaginatedResult<ProductResultDto>> GetAllProductsAsync(ProductSpecificationParams specifications)
        {
            var specs = new ProductWithFilterSpecification(specifications);
            var Countspecs = new ProductCountSpecification(specifications);

            var Products = await unitOfWork.GetRepository<Product, int>().GetAllAsync(specs);
            var TotalCount = await unitOfWork.GetRepository<Product, int>().CountAsync(Countspecs);
            var mappedProducts = mapper.Map<IEnumerable<ProductResultDto>>(Products);
            return new PaginatedResult<ProductResultDto>(specifications.PageIndex,specifications.PageSize, TotalCount, mappedProducts);
        }

        public async Task<ProductResultDto> GetProductByIdAsync(int id)
        {
            // specification pattern 
            var specs =new ProductWithFilterSpecification(id);

            var Product = await unitOfWork.GetRepository<Product, int>().GetAsync(specs);
            var mappedProduct = mapper.Map<ProductResultDto>(Product);
            return mappedProduct;
        }
    }
}
