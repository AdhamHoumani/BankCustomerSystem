using BankCustomerSystem.Services.Finance.Data.Enums;
using BankCustomerSystem.Services.Finance.Data.Models.DbModels;
using BankCustomerSystem.Services.Finance.Services.Models.Responses;
using AutoMapper;
using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankCustomerSystem.Services.Finance.Data.Models.Dto;
using BankCustomerSystem.Services.Finance.Service.Models.Requests;

namespace BankCustomerSystem.Services.Finance.Services.Models.Mapping
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

            CreateMap<Account, AccountDTO>()
                .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => _protector.Protect(src.Id.ToString())))
                .ForMember(dest => dest.CustomerId,
                opt => opt.MapFrom(src => _protector.Protect(src.CustomerId.ToString())))
                .ForMember(dest => dest.InitialBalance,
                opt => opt.MapFrom(src => src.InitialBalance))
                .ForMember(dest => dest.Balance,
                opt => opt.MapFrom(src => src.Balance))
                .ForMember(dest => dest.AccountStatusId,
                opt => opt.MapFrom(src => src.AccountStatusId))
                .ForMember(dest => dest.AccountTypeId,
                opt => opt.MapFrom(src => src.AccountTypeId))
                .ForMember(dest => dest.AccountNumber,
                opt => opt.MapFrom(src => src.AccountNumber))
                .ReverseMap()
                .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => int.Parse(_protector.Unprotect(src.Id.ToString()))))
                .ForMember(dest => dest.CustomerId,
                opt => opt.MapFrom(src => int.Parse(_protector.Unprotect(src.CustomerId.ToString()))))
                .ForMember(dest => dest.InitialBalance,
                opt => opt.MapFrom(src => src.InitialBalance))
                .ForMember(dest => dest.Balance,
                opt => opt.MapFrom(src => src.Balance))
                .ForMember(dest => dest.AccountStatusId,
                opt => opt.MapFrom(src => src.AccountStatusId))
                .ForMember(dest => dest.AccountTypeId,
                opt => opt.MapFrom(src => src.AccountTypeId))
                .ForMember(dest => dest.AccountNumber,
                opt => opt.MapFrom(src => src.AccountNumber));
        }
    }
}
