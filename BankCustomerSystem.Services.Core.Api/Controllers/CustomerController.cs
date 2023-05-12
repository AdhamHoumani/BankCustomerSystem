using BankCustomerSystem.Services.Core.Api.Helpers;
using BankCustomerSystem.Services.Core.Api.Models.Responses;
using BankCustomerSystem.Services.Core.Service.Interfaces;
using BankCustomerSystem.Services.Core.Service.Models.Requests;
using BankCustomerSystem.Services.Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankCustomerSystem.Services.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IRequestInfoService _requestInfoService;
        private readonly ResponseHelper _responseHelper;
        private readonly ICustomerService _customerService;
        public CustomerController(IRequestInfoService requestInfoService, ICustomerService customerService)
        {
            _requestInfoService = requestInfoService;
            _responseHelper = new ResponseHelper();
            _customerService = customerService;
        }

        [HttpPost("GetById")]
        public ActionResult<ApiResponse> GetCustomerbyId(GetCustomerByIdRequest request)
        {
            try
            {
                request.RequestInfoService = _requestInfoService;
                var result = _customerService.GetCustomerbyId(request);
                return _responseHelper.GenerateResponse(result, _requestInfoService.Language);
            }
            catch (Exception ex)
            {
                return _responseHelper.GenerateExceptionResponse(ex, _requestInfoService.Language);
            }
        }

        [HttpPost("GetAll")]
        public ActionResult<ApiResponse> GetALlCustomers(GetAllCustomersRequest request)
        {
            try
            {
                request.RequestInfoService = _requestInfoService;
                var result = _customerService.GetAllCustomers(request);
                return _responseHelper.GenerateResponse(result, _requestInfoService.Language);
            }
            catch (Exception ex)
            {
                return _responseHelper.GenerateExceptionResponse(ex, _requestInfoService.Language);
            }
        }

        [HttpPost("AddCustomer")]
        public ActionResult<ApiResponse> AddCustomer(AddCustomerRequest request)
        {
            try
            {
                request.RequestInfoService = _requestInfoService;
                var result = _customerService.AddCustomer(request);
                return _responseHelper.GenerateResponse(result, _requestInfoService.Language);
            }
            catch (Exception ex)
            {
                return _responseHelper.GenerateExceptionResponse(ex, _requestInfoService.Language);
            }
        }

        [HttpPost("UpdateCustomer")]
        public ActionResult<ApiResponse> UpdateCustomer(UpdateCustomerRequest request)
        {
            try
            {
                request.RequestInfoService = _requestInfoService;
                var result = _customerService.UpdateCustomer(request);
                return _responseHelper.GenerateResponse(result, _requestInfoService.Language);
            }
            catch (Exception ex)
            {
                return _responseHelper.GenerateExceptionResponse(ex, _requestInfoService.Language);
            }
        }

        [HttpPost("DeleteCustomer")]
        public async Task<ActionResult<ApiResponse>> DeleteCustomer(DeleteCustomerRequest request)
        {
            try
            {
                request.RequestInfoService = _requestInfoService;
                var result = await _customerService.DeleteCustomer(request);
                return _responseHelper.GenerateResponse(result, _requestInfoService.Language);
            }
            catch (Exception ex)
            {
                return _responseHelper.GenerateExceptionResponse(ex, _requestInfoService.Language);
            }
        }

        [HttpPost("GetCustomerAccounts")]
        public async Task<ActionResult<ApiResponse>> GetCustomerAccounts(GetCustomerAccountsRequest request)
        {
            try
            {
                request.RequestInfoService = _requestInfoService;
                var result = await _customerService.GetCustomerAccounts(request);
                return _responseHelper.GenerateResponse(result, _requestInfoService.Language);
            }
            catch (Exception ex)
            {
                return _responseHelper.GenerateExceptionResponse(ex, _requestInfoService.Language);
            }
        }
    }
}
