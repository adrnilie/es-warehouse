using EsWarehouse.Domain.Primitives;

namespace EsWarehouse.Domain.Events
{
    public class QuantityAdjusted : IEvent
    {
        public int Sku { get; set; }
        public int Version { get; set; }
        public int Quantity { get; set; }
        public string Type => typeof(QuantityAdjusted).FullName;
    }
}