using AutoMapper;
using Domain.Entities;
using Shared.BasketDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappedProfiles
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            CreateMap<CustomerBasket,BasketDto>().ReverseMap();
            CreateMap<BasketItem,ItemDto>().ReverseMap();

           
        }
    }
}
