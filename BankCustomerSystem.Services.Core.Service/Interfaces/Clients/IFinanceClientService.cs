using BankCustomerSystem.Services.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankCustomerSystem.Services.Core.Service.Interfaces.Clients
{
    public interface IFinanceClientService
    {
        Task<TResponse> PostAsync<TRequest, TResponse>(string uri, TRequest request, IRequestInfoService requestInfoService, CancellationToken cancellationToken);
    }
}
