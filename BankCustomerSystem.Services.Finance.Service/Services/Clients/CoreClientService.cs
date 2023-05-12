using BankCustomerSystem.Services.Finance.Service.Interfaces.Clients;
using BankCustomerSystem.Services.Finance.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankCustomerSystem.Services.Finance.Service.Services.Clients
{
    public class CoreClientService : ICoreClientService
    {
        private readonly HttpClient _httpClient;
        private readonly IRequestInfoService _requestInfoService;
        public CoreClientService(HttpClient httpClient, IRequestInfoService requestInfoService)
        {
            _httpClient = httpClient;
            _requestInfoService = requestInfoService;
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string uri, TRequest request, IRequestInfoService requestInfoService, CancellationToken cancellationToken)
        {
            setHttpHeaders(requestInfoService ?? _requestInfoService);
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
            var jsonContent = JsonConvert.SerializeObject(request);
            httpRequestMessage.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            return await Post<TResponse>(httpRequestMessage, cancellationToken);
        }

        private async Task<TResponse> Post<TResponse>(HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken)
        {
            var response = await _httpClient.SendAsync(httpRequestMessage, cancellationToken);
            response.EnsureSuccessStatusCode();
            var responseObject = JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync());
            return responseObject;
        }

        private void setHttpHeaders(IRequestInfoService requestInfoService)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("language", requestInfoService.Language);
        }
    }
}
