using BankCustomerSystem.Services.Finance.Services.Models.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankCustomerSystem.Services.Finance.Services.Models.Responses
{
    public class BaseServiceResponse
    {
        public dynamic? Data { get; set; }
        public int Status { get; set; }
        public List<MessageDTO> UserMessages { get; set; } = new List<MessageDTO>();
        public List<MessageDTO> TechMessages { get; set; } = new List<MessageDTO>();
        public bool SendDirectly { get; set; } = false;
    }
}
