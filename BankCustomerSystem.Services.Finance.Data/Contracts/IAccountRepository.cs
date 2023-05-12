using BankCustomerSystem.Services.Finance.Data.Models.DbModels;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankCustomerSystem.Services.Finance.Data.Contracts
{
    public interface IAccountRepository : IGenericRepository<Account,SqlConnection>
    {
    }
}
