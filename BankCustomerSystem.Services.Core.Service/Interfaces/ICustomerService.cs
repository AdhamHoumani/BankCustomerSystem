using BankCustomerSystem.Services.Core.Service.Models.Requests;
using BankCustomerSystem.Services.Core.Services.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankCustomerSystem.Services.Core.Service.Interfaces
{
    public interface ICustomerService
    {
        BaseServiceResponse GetAllCustomers(GetAllCustomersRequest request);
        BaseServiceResponse GetCustomerbyId(GetCustomerByIdRequest request);
        BaseServiceResponse AddCustomer(AddCustomerRequest request);
        BaseServiceResponse UpdateCustomer(UpdateCustomerRequest request);
        Task<BaseServiceResponse> DeleteCustomer(DeleteCustomerRequest request);
        Task<BaseServiceResponse> GetCustomerAccounts(GetCustomerAccountsRequest request);
    }
}
