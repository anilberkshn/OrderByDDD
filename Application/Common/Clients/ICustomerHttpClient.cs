using System;
using System.Threading.Tasks;

namespace Application.Common.Clients
{
    public interface ICustomerHttpClient
    {
        public Task<CustomerClientModel> GetCustomerFromCustomerApi(Guid customerId);
        public Task <bool> CheckCustomerId(Guid customerId);
    }
}