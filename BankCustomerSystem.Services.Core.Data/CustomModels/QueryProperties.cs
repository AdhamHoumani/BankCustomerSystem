using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankCustomerSystem.Services.Core.Data.CustomModels
{
    public class QueryProperties
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public List<WhereField> Where { get; set; } = new List<WhereField>();
        public List<OrderByField> OrderBy { get; set; } = new List<OrderByField>();
    }
}
