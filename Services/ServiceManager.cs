using AutoMapper;
using Domain.Contracts;
using Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<IProductService> _productService;
        private readonly Lazy<IBasketService> _BasketService;

        public ServiceManager(IUnitOfWork unitOfWork, IMapper mapper, IBasketRepository basketRepository)
        {
            _productService = new Lazy<IProductService>(() => new ProductService(unitOfWork, mapper));

            _BasketService = new Lazy<IBasketService>(() => new BasketService(basketRepository, mapper));
        }

        public IProductService productService => _productService.Value;

        public IBasketService BasketService => _BasketService.Value;
    }
}
