using System;

namespace MediatrEntityHandlerDemo.Domain.Entities
{
    public abstract class BaseEntity
    {
        public DateTimeOffset DateCreatedUtc { get; set; }

        public DateTimeOffset DateUpdatedUtc { get; set; }

        public string CreatedByUser { get; set; }

        public string UpdatedByUser { get; set; }
    }
}

