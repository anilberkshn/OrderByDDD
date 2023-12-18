using System;

namespace Application.Common.Models.Dto
{
    public class SoftDeleteOrderDto
    {
        public DateTime DeletedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}