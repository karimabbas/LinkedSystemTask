using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LinkSystem.Dto;
using LinkSystem.Models;

namespace LinkSystem.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Prodcut, ProdcutDto>();
        }
    }
}