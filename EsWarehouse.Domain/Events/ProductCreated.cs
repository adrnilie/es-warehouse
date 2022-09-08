using EsWarehouse.Domain.Primitives;

namespace EsWarehouse.Domain.Events
{
    public class ProductCreated : IEvent
    {
        public int Sku { get; set; }
        public string Name { get; set; } = default!;
        public int Quantity { get; set; }
        public int Version { get; set; }
        public string Type => typeof(ProductCreated).FullName;
    }
}