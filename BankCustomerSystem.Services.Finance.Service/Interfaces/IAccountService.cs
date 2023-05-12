using BankCustomerSystem.Services.Finance.Service.Models.Requests;
using BankCustomerSystem.Services.Finance.Services.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankCustomerSystem.Services.Finance.Service.Interfaces
{
    public interface IAccountService
    {
        BaseServiceResponse GetCustomerAccounts(GetCustomerAccountsRequest request);
    }
}
