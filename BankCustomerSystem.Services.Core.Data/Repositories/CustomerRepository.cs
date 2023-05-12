using BankCustomerSystem.Services.Core.Data.Contracts;
using BankCustomerSystem.Services.Core.Data.Models.DbModels;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankCustomerSystem.Services.Core.Data.Repositories
{
    public class CustomerRepository : GenericRepository<Customer, SqlConnection>, ICustomerRepository
    {
        public CustomerRepository(string connectionString) : base(connectionString)
        {
        }
    }
}
