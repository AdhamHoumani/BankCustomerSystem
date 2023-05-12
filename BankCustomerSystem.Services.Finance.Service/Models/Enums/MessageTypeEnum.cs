using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankCustomerSystem.Services.Finance.Services.Models.Enums
{
    public enum MessageTypeEnum
    {
        Success = 1,
        Information = 2,
        Warning = 0,
        Error = -1,
        Exception = -2
    }
}
