using System;

namespace Domain.Entities
{
    public class Product
    {
        public Guid ProductId { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
    }
}