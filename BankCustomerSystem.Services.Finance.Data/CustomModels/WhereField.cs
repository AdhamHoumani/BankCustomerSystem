using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankCustomerSystem.Services.Finance.Data.CustomModels
{
    public class WhereField
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public int Operation { get; set; }

        public WhereField(string name, string value)
        {
            Name = name;
            Value = value;
            Operation = Operation;
        }

        public WhereField()
        {
            Name = Name;
            Value = Value;
            Operation = Operation;
        }
    }
}
