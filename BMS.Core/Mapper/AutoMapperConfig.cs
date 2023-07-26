using AutoMapper;
using BMS.Core.DTO;
using BMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.Mapper
{
    internal class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Post, CreatePostDTO>()
                .ForMember(x => x.Title, y => y.MapFrom(b => b.Title))
                .ForMember(x => x.Content, y => y.MapFrom(b => b.Content)).ReverseMap();

            CreateMap<User, RegisterUserDTO>();
        }
    }
}
