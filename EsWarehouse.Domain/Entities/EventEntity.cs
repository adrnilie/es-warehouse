using System;

namespace EsWarehouse.Domain.Entities
{
    public class EventEntity
    {
        public Guid Id { get; set; }
        public int CorrelationId { get; set; }
        public string Body { get; set; } = default!;
        public string Type { get; set; } = default!;
        public int Version { get; set; }
        public DateTime CreationDate { get; set; }
    }
}