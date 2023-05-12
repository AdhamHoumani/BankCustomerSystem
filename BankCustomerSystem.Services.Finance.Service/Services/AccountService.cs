using AutoMapper;
using BankCustomerSystem.Services.Finance.Data.Contracts;
using BankCustomerSystem.Services.Finance.Data.CustomModels;
using BankCustomerSystem.Services.Finance.Data.Enums;
using BankCustomerSystem.Services.Finance.Data.Models.DbModels;
using BankCustomerSystem.Services.Finance.Data.Models.Dto;
using BankCustomerSystem.Services.Finance.Service.Interfaces;
using BankCustomerSystem.Services.Finance.Service.Interfaces.Clients;
using BankCustomerSystem.Services.Finance.Service.Models.Requests;
using BankCustomerSystem.Services.Finance.Services.Models.Custom;
using BankCustomerSystem.Services.Finance.Services.Models.Enums;
using BankCustomerSystem.Services.Finance.Services.Models.Responses;
using Microsoft.AspNetCore.DataProtection;
using RepoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankCustomerSystem.Services.Finance.Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        public AccountService(IAccountRepository accountRepository, 
            IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        public BaseServiceResponse GetCustomerAccounts(GetCustomerAccountsRequest request)
        {
            var accounts = _accountRepository.GetByCondition(new RepoDbQueryProperties(new QueryProperties
            {
                PageIndex = 1,
                PageSize = 0,
                Where = new List<WhereField> { new WhereField { Name = nameof(Account.CustomerId), Value = request.CustomerId, Operation = (int)OperationEnum.Equal } },
                OrderBy = new()
            }),null).ToList();
            var accountsDTO = _mapper.Map<List<AccountDTO>>(accounts);
            return new BaseServiceResponse
            {
                Data = accountsDTO,
                Status = (int)ServiceResponseStatusEnum.Success,
                SendDirectly = true
            };
        }
    }
}
