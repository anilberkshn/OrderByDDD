using Domain.Entities;

namespace Application.Common.Models.Request
{
    public class UpdateRequestModel
    {
        // public Guid CustomerId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string Status { get; set; }
        public Product Type { get; set; }
    }
}