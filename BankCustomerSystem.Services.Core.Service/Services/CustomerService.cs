using AutoMapper;
using BankCustomerSystem.Services.Core.Data.Contracts;
using BankCustomerSystem.Services.Core.Data.CustomModels;
using BankCustomerSystem.Services.Core.Data.Enums;
using BankCustomerSystem.Services.Core.Data.Models.DbModels;
using BankCustomerSystem.Services.Core.Data.Models.Dto;
using BankCustomerSystem.Services.Core.Service.Interfaces;
using BankCustomerSystem.Services.Core.Service.Interfaces.Clients;
using BankCustomerSystem.Services.Core.Service.Models.Requests;
using BankCustomerSystem.Services.Core.Service.Models.Requests.Finance;
using BankCustomerSystem.Services.Core.Services.Models.Custom;
using BankCustomerSystem.Services.Core.Services.Models.Enums;
using BankCustomerSystem.Services.Core.Services.Models.Responses;
using Microsoft.AspNetCore.DataProtection;
using RepoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankCustomerSystem.Services.Core.Service.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private List<MessageDTO> messagesDTOs = new();
        private readonly IMapper _mapper;
        private readonly IDataProtector _protector;
        private readonly IFinanceClientService _financeClientService;

        public CustomerService(ICustomerRepository customerRepository,
            IDataProtector protector,
            IMapper mapper,
            IFinanceClientService financeClientService)
        {
            _customerRepository = customerRepository;
            _protector = protector;
            _mapper = mapper;
            _financeClientService = financeClientService;
        }

        public BaseServiceResponse GetAllCustomers(GetAllCustomersRequest request)
        {
            messagesDTOs = new();
            var dbCustomers = _customerRepository.GetAll(new RepoDbQueryProperties(), null);
            var customersDTO = _mapper.Map<List<CustomerDTO>>(dbCustomers);
            return new BaseServiceResponse
            {
                Data = customersDTO,
                Status = (int)ServiceResponseStatusEnum.Success,
            };
        }

        public BaseServiceResponse GetCustomerbyId(GetCustomerByIdRequest request)
        {
            messagesDTOs = new();
            var customerId = int.Parse(_protector.Unprotect(request.CustomerId));
            var dbCustomer = _customerRepository.GetById(customerId, null);
            if (dbCustomer is null)
            {
                messagesDTOs.Add(new MessageDTO
                {
                    Message = UserMessagesKeys.EntityNotFound.ToString(),
                    Type = (int)MessageTypeEnum.Error
                });
                return new BaseServiceResponse
                {
                    Data = null,
                    Status = (int)ServiceResponseStatusEnum.Failed,
                    UserMessages = messagesDTOs
                };
            }
            var customerDTO = _mapper.Map<CustomerDTO>(dbCustomer);
            return new BaseServiceResponse
            {
                Data = customerDTO,
                Status = (int)ServiceResponseStatusEnum.Success,
            };
        }

        public BaseServiceResponse AddCustomer(AddCustomerRequest request)
        {
            messagesDTOs = checkCredentialsIfUnique(request.Email,request.PhoneNumber);
            if(messagesDTOs.Count > 0)
            {
                return new BaseServiceResponse
                {
                    Data = null,
                    Status = (int)ServiceResponseStatusEnum.Failed,
                    UserMessages = messagesDTOs
                };
            }
            var customer = _mapper.Map<Customer>(request);
            customer.Id = _customerRepository.Insert<int>(customer, null);
            var customerDTO = _mapper.Map<CustomerDTO>(customer);
            messagesDTOs.Add(new MessageDTO
            {
                Message = UserMessagesKeys.AddedSuccessfully.ToString(),
                Type = (int)MessageTypeEnum.Success
            });
            return new BaseServiceResponse
            {
                Data = customerDTO,
                Status = (int)ServiceResponseStatusEnum.Success,
                UserMessages = messagesDTOs
            };
        }

        public async Task<BaseServiceResponse> DeleteCustomer(DeleteCustomerRequest request)
        {
            messagesDTOs = new();
            int customerId = int.Parse(_protector.Unprotect(request.CustomerId));
            if (request.WithCheckingAccounts)
            {
                var financeRequest = new CheckCustomerActiveAccountsRequest
                {
                    CustomerId = customerId
                };
                var financeResponse = await _financeClientService.PostAsync<CheckCustomerActiveAccountsRequest, BaseClietResponse<bool>>("Account/CheckCustomerActiveAccountsRequest", financeRequest, request.RequestInfoService, default);
                if (financeResponse.ApiStatus != (int)ClientResponseStatus.Success)
                {
                    messagesDTOs.Add(new MessageDTO
                    {
                        Message = UserMessagesKeys.CannotDeleteThisCustomer.ToString(),
                        Type = (int)MessageTypeEnum.Error
                    });
                    return new BaseServiceResponse
                    {
                        Data = null,
                        Status = (int)ServiceResponseStatusEnum.Failed,
                        UserMessages = messagesDTOs
                    };
                }
                if (financeResponse.Data)
                {
                    messagesDTOs.Add(new MessageDTO
                    {
                        Message = UserMessagesKeys.CustomerHasActiveAccounts.ToString(),
                        Type = (int)MessageTypeEnum.Warning
                    });
                    return new BaseServiceResponse
                    {
                        Data = null,
                        Status = (int)ServiceResponseStatusEnum.Failed,
                        UserMessages = messagesDTOs
                    };
                }

            }
            _customerRepository.Delete(customerId);
            messagesDTOs.Add(new MessageDTO
            {
                Message = UserMessagesKeys.DeletedSuccessfully.ToString(),
                Type = (int)MessageTypeEnum.Success
            });
            return new BaseServiceResponse
            {
                Status = (int)ServiceResponseStatusEnum.Success,
                Data = true,
                UserMessages = messagesDTOs
            };
        }

        public BaseServiceResponse UpdateCustomer(UpdateCustomerRequest request)
        {
            messagesDTOs = checkCredentialsIfUnique(request.Email, request.PhoneNumber);
            if (messagesDTOs.Count > 0)
            {
                return new BaseServiceResponse
                {
                    Data = null,
                    Status = (int)ServiceResponseStatusEnum.Failed,
                    UserMessages = messagesDTOs
                };
            }
            var customerToUpdate = _mapper.Map<Customer>(request);
            var fieldsToUpdate = new List<Field>
            {
                new Field(nameof(Customer.Id)),
                new Field(nameof(Customer.FirstName)),
                new Field(nameof(Customer.LastName)),
                new Field(nameof(Customer.Email)),
                new Field(nameof(Customer.PhoneNumber)),
                new Field(nameof(Customer.Address)),
            };
            _customerRepository.Update(customerToUpdate, fieldsToUpdate, null);
            messagesDTOs.Add(new MessageDTO
            {
                Message = UserMessagesKeys.UpdatedSuccessfully.ToString(),
                Type = (int)MessageTypeEnum.Success
            });
            return new BaseServiceResponse
            {
                Data = _mapper.Map<CustomerDTO>(customerToUpdate),
                UserMessages = messagesDTOs,
                Status = (int) ServiceResponseStatusEnum.Success
            };
        }

        public async Task<BaseServiceResponse> GetCustomerAccounts(Models.Requests.GetCustomerAccountsRequest request)
        {
            messagesDTOs = new();
            var financeRequest = new Models.Requests.Finance.GetCustomerAccountsRequest
            {
                CustomerId = int.Parse(_protector.Unprotect(request.CustomerId))
            };
            var financeResponse = await _financeClientService.PostAsync<Models.Requests.Finance.GetCustomerAccountsRequest, BaseServiceResponse>
                ("Account/GetCustomerAccounts",financeRequest,request.RequestInfoService,default);
            if(financeResponse != null)
            {
                return financeResponse;
            }
            return new BaseServiceResponse
            {
                Data = null,
                Status = (int)ServiceResponseStatusEnum.Failed
            };
        }

        private List<MessageDTO> checkCredentialsIfUnique(string email, string phoneNumber)
        {
            messagesDTOs = new();
            var customer = _customerRepository.GetByCondition(new RepoDbQueryProperties(new QueryProperties
            {
                PageIndex = 1,
                PageSize = 1,
                Where = new List<WhereField> { new WhereField { Name = nameof(Customer.Email), Value = email, Operation = (int)OperationEnum.Equal } },
                OrderBy = new List<OrderByField>()
            }), null);
            if(customer != null)
            {
                messagesDTOs.Add(new MessageDTO
                {
                    Message = UserMessagesKeys.EmailAlreadyUsed.ToString(),
                    Type = (int)MessageTypeEnum.Warning
                });
            }
            
            customer = _customerRepository.GetByCondition(new RepoDbQueryProperties(new QueryProperties
            {
                PageIndex = 1,
                PageSize = 1,
                Where = new List<WhereField> { new WhereField { Name = nameof(Customer.PhoneNumber), Value = email, Operation = (int)OperationEnum.Equal } },
                OrderBy = new List<OrderByField>()
            }), null);
            if (customer != null)
            {
                messagesDTOs.Add(new MessageDTO
                {
                    Message = UserMessagesKeys.PhoneNumberAlreadyUsed.ToString(),
                    Type = (int)MessageTypeEnum.Warning
                });
            }

            return messagesDTOs;
        }
    }
}
