using AutoMapper;
using Domain.Entities.Identity;
using Shared.IdentityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappedProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserResultDto,User>().ReverseMap();
            //CreateMap<User, UserResultDto>().ForMember(d => d.Token ,o =>o.MapFrom(s =>s.));
        }
    }
}
