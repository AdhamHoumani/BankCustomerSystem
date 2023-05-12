using BankCustomerSystem.Services.Finance.Api.Helpers;
using BankCustomerSystem.Services.Finance.Api.Models.Responses;
using BankCustomerSystem.Services.Finance.Service.Interfaces;
using BankCustomerSystem.Services.Finance.Service.Models.Requests;
using BankCustomerSystem.Services.Finance.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankCustomerSystem.Services.Finance.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IRequestInfoService _requestInfoService;
        private readonly ResponseHelper _responseHelper;
        private readonly IAccountService _accountService;
        public AccountController(IRequestInfoService requestInfoService,
            IAccountService accountService)
        {
            _requestInfoService = requestInfoService;
            _responseHelper = new ResponseHelper();
            _accountService = accountService;
        }

        [HttpPost("GetCustomerAccounts")]
        ActionResult<ApiResponse> GetCustomerAccounts(GetCustomerAccountsRequest request)
        {
            try
            {
                request.RequestInfoService = _requestInfoService;
                var result = _accountService.GetCustomerAccounts(request);
                return _responseHelper.GenerateResponse(result, _requestInfoService.Language);
            }
            catch (Exception ex)
            {
                return _responseHelper.GenerateExceptionResponse(ex, _requestInfoService.Language);
            }
        }

    }
}
