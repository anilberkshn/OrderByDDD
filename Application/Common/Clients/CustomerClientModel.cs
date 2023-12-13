using Domain.Entities;

namespace Application.Common.Clients
{
    public class CustomerClientModel
    {
        public string Name  { get; set; }
        public string Email { get; set; }
        public Address Address { get; set; }
    }
} 