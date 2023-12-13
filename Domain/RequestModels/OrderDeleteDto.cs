using System;
using Domain.Entities;

namespace Domain.RequestModels
{
    public class OrderDeleteDto
    {
        public Guid CustomerId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string Status { get; set; }
        public Product Product { get; set; }
        public Address Address { get; set; }

    }
}