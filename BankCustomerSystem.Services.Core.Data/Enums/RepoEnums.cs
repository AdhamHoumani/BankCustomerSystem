using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankCustomerSystem.Services.Core.Data.Enums
{
    public enum OperationEnum
    {
        Equal = 1,
        NotEqual = 2,
        Like = 3,
        NotLike = 4,
        Greater = 5,
        Less = 6,
        GreaterOrEqual = 7,
        LessOrEqual = 8,
        In = 9,
        NotIn = 10,
    }

    public enum OrderByEnum
    {
        Ascending = 1,
        Descending = 2,
    }
}
