using BankCustomerSystem.Services.Core.Data.Enums;
using BankCustomerSystem.Services.Core.Data.Models.DbModels;
using BankCustomerSystem.Services.Core.Services.Models.Responses;
using AutoMapper;
using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankCustomerSystem.Services.Core.Data.Models.Dto;
using BankCustomerSystem.Services.Core.Service.Models.Requests;

namespace BankCustomerSystem.Services.Core.Services.Models.Mapping
{
    public class MapperProfile : Profile
    {
        private readonly IDataProtector _protector;

        public MapperProfile(IDataProtector protector)
        {
            _protector = protector;

            CreateMap<BaseClietResponse<dynamic>, BaseServiceResponse>()
                .ForMember(des => des.Status,
                opt => opt.MapFrom(src => src.ApiStatus))
                .ForMember(des => des.UserMessages,
                opt => opt.MapFrom(src => src.UserMessages))
                .ForMember(des => des.TechMessages,
                opt => opt.MapFrom(src => src.TechMessages))
                .ForMember(des => des.Data,
                opt => opt.MapFrom(src => src.Data))
                .ForMember(des => des.SendDirectly,
                opt => opt.MapFrom(src => true));

            CreateMap<Customer, CustomerDTO>()
               .ForMember(des => des.Id,
               opt => opt.MapFrom(src => _protector.Protect(src.Id.ToString())))
               .ForMember(des => des.FirstName,
               opt => opt.MapFrom(src => src.FirstName))
               .ForMember(des => des.LastName,
               opt => opt.MapFrom(src => src.LastName))
               .ForMember(des => des.Email,
               opt => opt.MapFrom(src => src.Email))
               .ForMember(des => des.PhoneNumber,
               opt => opt.MapFrom(src => src.PhoneNumber))
               .ForMember(des => des.Address,
               opt => opt.MapFrom(src => src.Address))
               .ReverseMap()
               .ForMember(des => des.Id,
               opt => opt.MapFrom(src => int.Parse(_protector.Unprotect(src.Id.ToString()))))
               .ForMember(des => des.FirstName,
               opt => opt.MapFrom(src => src.FirstName))
               .ForMember(des => des.LastName,
               opt => opt.MapFrom(src => src.LastName))
               .ForMember(des => des.Email,
               opt => opt.MapFrom(src => src.Email))
               .ForMember(des => des.PhoneNumber,
               opt => opt.MapFrom(src => src.PhoneNumber))
               .ForMember(des => des.Address,
               opt => opt.MapFrom(src => src.Address));

            CreateMap<AddCustomerRequest, Customer>()
               .ForMember(des => des.FirstName,
               opt => opt.MapFrom(src => src.FirstName))
               .ForMember(des => des.LastName,
               opt => opt.MapFrom(src => src.LastName))
               .ForMember(des => des.Email,
               opt => opt.MapFrom(src => src.Email))
               .ForMember(des => des.PhoneNumber,
               opt => opt.MapFrom(src => src.PhoneNumber))
               .ForMember(des => des.Address,
               opt => opt.MapFrom(src => src.Address));

            CreateMap<UpdateCustomerRequest, Customer>()
               .ForMember(des => des.Id,
               opt => opt.MapFrom(src => int.Parse(src.CustomerId) ))
               .ForMember(des => des.FirstName,
               opt => opt.MapFrom(src => src.FirstName))
               .ForMember(des => des.LastName,
               opt => opt.MapFrom(src => src.LastName))
               .ForMember(des => des.Email,
               opt => opt.MapFrom(src => src.Email))
               .ForMember(des => des.PhoneNumber,
               opt => opt.MapFrom(src => src.PhoneNumber))
               .ForMember(des => des.Address,
               opt => opt.MapFrom(src => src.Address));
        }
    }
}
