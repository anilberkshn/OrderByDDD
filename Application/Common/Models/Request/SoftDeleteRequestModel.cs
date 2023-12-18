using System;

namespace Application.Common.Models.Request
{
    public class SoftDeleteRequestModel
    {
        public DateTime DeletedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}