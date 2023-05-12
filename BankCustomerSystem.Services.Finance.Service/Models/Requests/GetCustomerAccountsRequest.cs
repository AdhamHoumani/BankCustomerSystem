using BankCustomerSystem.Services.Finance.Services.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankCustomerSystem.Services.Finance.Service.Models.Requests
{
    public class GetCustomerAccountsRequest : BaseRequest
    {
        public int CustomerId { get; set; }
    }
}
