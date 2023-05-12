﻿using BankCustomerSystem.Services.Core.Services.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankCustomerSystem.Services.Core.Service.Models.Requests
{
    public class GetCustomerByIdRequest : BaseRequest
    {
        public string CustomerId { get; set; } = string.Empty;
    }
}
