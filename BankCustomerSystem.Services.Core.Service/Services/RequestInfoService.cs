using BankCustomerSystem.Services.Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;


namespace BankCustomerSystem.Services.Core.Services.Services
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
