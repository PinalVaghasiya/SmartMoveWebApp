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
            Mapper.CreateMap<CustomerRating, CustomerRatingDto>();
            Mapper.CreateMap<TruckOwner, TruckOwnerDto>();
            Mapper.CreateMap<TruckOwner, TruckOwnerDto>().ReverseMap();
            Mapper.CreateMap<Order, OrderDto>();
            Mapper.CreateMap<OrderBid, OrderBidDto>();

            //DTO to Domain
            Mapper.CreateMap<CustomerDto, Customer>();
            Mapper.CreateMap<CustomerRatingDto, CustomerRating>();
            Mapper.CreateMap<TruckOwnerDto, TruckOwner>();
            Mapper.CreateMap<OrderDto, Order>();
            Mapper.CreateMap<OrderBidDto, OrderBid>();
        }
    }
}