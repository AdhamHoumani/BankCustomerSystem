using BankCustomerSystem.Services.Finance.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankCustomerSystem.Services.Finance.Service.Interfaces.Clients
{
    public interface ICoreClientService
    {
        Task<TResponse> PostAsync<TRequest, TResponse>(string uri, TRequest request, IRequestInfoService requestInfoService, CancellationToken cancellationToken);
    }
}
