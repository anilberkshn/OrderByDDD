using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Application.Common.Models.Error;
using Newtonsoft.Json;

namespace Application.Common.Clients
{
    public class CustomerHttpClient : ICustomerHttpClient
    {
        private readonly HttpClient _httpClient;
        public CustomerHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<CustomerClientModel> GetCustomerFromCustomerApi(Guid customerId)
        {
            var response = await _httpClient.GetAsync($"api/customers/{customerId}");
            var jsonConvert = JsonConvert.SerializeObject(response);
            if (response.IsSuccessStatusCode)
            {
                var customer = await response.Content.ReadFromJsonAsync<CustomerClientModel>();
                return customer;
            }
            else
            {
                throw new CustomException(HttpStatusCode.NotFound, "Customer bulunamadı");
            }
           
        }
        public async Task<bool> CheckCustomerId(Guid customerId)
        {
           var response = await _httpClient.GetAsync($"api/customers/{customerId}");
            
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
            
        }
    }
}