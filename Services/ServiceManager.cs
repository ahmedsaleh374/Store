using AutoMapper;
using Domain.Contracts;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Services.Abstractions;
using Shared.IdentityDtos;
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
        private readonly Lazy<IAuthenticationService> _authenticationService;

        public ServiceManager(IUnitOfWork unitOfWork
            , IMapper mapper
            , IBasketRepository basketRepository
            ,UserManager<User> userManager
            , ILogger<AuthenticationService> logger
            ,IOptions<JwtOptions> options)
        {
            _productService = new Lazy<IProductService>(() => new ProductService(unitOfWork, mapper));

            _BasketService = new Lazy<IBasketService>(() => new BasketService(basketRepository, mapper));

            _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(userManager, mapper, logger, options));
        }

        public IProductService productService => _productService.Value;

        public IBasketService BasketService => _BasketService.Value;

        public IAuthenticationService authenticationService => _authenticationService.Value;
    }
}
