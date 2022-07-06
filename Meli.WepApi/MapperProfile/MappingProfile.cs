using AutoMapper;
using Meli.Common.Entities;
using Meli.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meli.WepApi.MapperProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Individual, IndividualModel>()
               .ForMember(d => d.Dna, o => o.MapFrom(c => c.Dna))
               .ReverseMap();
        }

    }
}
