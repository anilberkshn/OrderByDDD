using System;
using Domain.Entities;

namespace Domain.RequestModels
{
    public class CreateDto
    {
        public Guid CustomerId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public Product Product { get; set; }
    }
}