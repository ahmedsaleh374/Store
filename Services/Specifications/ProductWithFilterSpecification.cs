using Domain.Contracts;
using Domain.Entities;
using Shared.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    public class ProductWithFilterSpecification : Specification<Product>
    {
        public ProductWithFilterSpecification(int id) : base(p => p.Id == id)
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
        }

        public ProductWithFilterSpecification(ProductSpecificationParams specs)
            : base(p => (!specs.BrandId.HasValue || p.BrandId == specs.BrandId) &&
                       (!specs.TypeId.HasValue || p.TypeId == specs.TypeId) &&
                       (string.IsNullOrWhiteSpace(specs.Search) || p.Name.ToLower().Contains(specs.Search.ToLower().Trim())) 
                   )
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);

            ApplyPagination(specs.PageIndex,specs.PageSize);

            if (specs.Sort != null) 
            {
                switch (specs.Sort) 
                {
                    case SortingOptions.NameAsc:
                        SetOrderBy(p => p.Name);
                        break;
                    case SortingOptions.NameDesc:
                        SetOrderByDescending(p => p.Name);
                        break;
                    case SortingOptions.PriceAsc:
                        SetOrderBy(p => p.Price);
                        break;
                    case SortingOptions.PriceDesc:
                        SetOrderByDescending(p => p.Price);
                        break;
                    default :
                            SetOrderBy(p => p.Name);
                        break;
                }
            }
        }

    }
}
