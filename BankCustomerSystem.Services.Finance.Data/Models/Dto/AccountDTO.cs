using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankCustomerSystem.Services.Finance.Data.Models.Dto
{
    public class AccountDTO
    {
        public string Id { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public int AccountStatusId { get; set; }
        public int AccountTypeId { get; set; }
        public decimal Balance { get; set; }
        public decimal InitialBalance { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
    }
}
