using System;
using Domain.Entities;

namespace Application.Common.Models.Request
{
    public class CreateRequestModel
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public Product Product { get; set; }
    }
}