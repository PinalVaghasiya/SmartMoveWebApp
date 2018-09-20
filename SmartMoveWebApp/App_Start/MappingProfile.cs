using AutoMapper;
using SmartMoveWebApp.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartMoveWebApp.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Domain to DTO
            Mapper.CreateMap<Customer, CustomerDto>();
            Mapper.CreateMap<TruckOwner, TruckOwnerDto>();
            
            //DTO to Domain
        }
    }
}