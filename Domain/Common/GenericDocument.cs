using System;

namespace Domain.Common
{
    public class GenericDocument
    {
        public Guid Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public DateTime DeleteTime { get; set; }
        public bool IsDeleted { get; set; } 
    }
}