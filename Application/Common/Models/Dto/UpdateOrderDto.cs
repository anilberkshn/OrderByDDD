using Domain.Entities;

namespace Application.Common.Models.Dto
{
    public class UpdateOrderDto
    {
        // public Guid CustomerId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string Status { get; set; }
        public Product Product { get; set; }
    }
}