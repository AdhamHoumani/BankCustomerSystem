using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankCustomerSystem.Services.Core.Services.Models.Custom
{
    public class MessageDTO
    {
        public string Message { get; set; } = String.Empty; 
        public int Type { get; set; }
        public List<string> Parameters { get; set; } = new List<string>();
    }
}
