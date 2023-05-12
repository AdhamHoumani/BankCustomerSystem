using BankCustomerSystem.Services.Finance.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankCustomerSystem.Services.Finance.Services.Models.Requests
{
    public class BaseRequest
    {
        #nullable disable
        public IRequestInfoService RequestInfoService { get; set; } = null;
    }
}
