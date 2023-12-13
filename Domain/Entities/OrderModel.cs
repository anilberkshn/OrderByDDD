using System;
using Domain.Common;

namespace Domain.Entities
{
    public class OrderModel: GenericDocument
    {
        public Guid CustomerId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string Status { get; set; }
        public Product Product { get; set; }

        public Address Address { get; set; }
        

    }
}