using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Exceptions;
using Services.Abstractions;
using Shared.BasketDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class BasketService(IBasketRepository basketRepository , IMapper mapper) : IBasketService
    {
        public async Task<bool> DeleteBasketAsync(string id)
          => await basketRepository.DeleteBasketAsync(id);

        public async Task<BasketDto> GetBasketAsync(string id)
        {
            var basket = await basketRepository.GetBasketAsync(id);
            var mappedBasket = mapper.Map<BasketDto>(basket);
            return basket is  null ? throw new BasketNotFoundException(id) : mappedBasket;
        }

        public async Task<BasketDto> UpdateBasketAsync(BasketDto basket)
        {
            var customerBasket = mapper.Map<CustomerBasket>(basket);

            var UpdateBasket = await basketRepository.UpdateBasketAsync(customerBasket);

            var basketDto = mapper.Map<BasketDto>(UpdateBasket);

            return UpdateBasket is null ? throw new Exception("Can not updated basket now !") :basketDto ;

        }
    }
}
