using System;
using Domain.Common;
using Domain.Entities;

namespace Application.Common.Models.Dto
{
    public class CreateOrderDto

    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public Product Product { get; set; }
        public Address Address { get; set; }
    }
}