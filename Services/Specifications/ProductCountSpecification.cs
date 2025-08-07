using Domain.Contracts;
using Domain.Entities;
using Shared.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    public class ProductCountSpecification : Specification<Product>
    {
        public ProductCountSpecification(ProductSpecificationParams specs)
            : base(p => (!specs.BrandId.HasValue || p.BrandId == specs.BrandId) &&
                       (!specs.TypeId.HasValue || p.TypeId == specs.TypeId) &&
                       (string.IsNullOrWhiteSpace(specs.Search) || p.Name.ToLower().Contains(specs.Search.ToLower().Trim()))
                   )
        {

        }
    }
}
