using BankCustomerSystem.Services.Finance.Api.Helpers;
using BankCustomerSystem.Services.Finance.Service.Interfaces;
using BankCustomerSystem.Services.Finance.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankCustomerSystem.Services.Finance.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IRequestInfoService _requestInfoService;
        private readonly ResponseHelper _responseHelper;
        public TransactionController(IRequestInfoService requestInfoService)
        {
            _requestInfoService = requestInfoService;
            _responseHelper = new ResponseHelper();
        }
    }
}
