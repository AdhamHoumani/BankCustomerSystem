using BankCustomerSystem.Services.Finance.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankCustomerSystem.Services.Finance.Services.Services
{
    public class RequestInfoService : IRequestInfoService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public string Language { get; }

        public RequestInfoService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            var headers = httpContextAccessor.HttpContext.Request.Headers;
            Language = headers.ContainsKey("language") ? headers.FirstOrDefault(h=>h.Key== "language").Value.ToString() : "en";
        }
    }
}
