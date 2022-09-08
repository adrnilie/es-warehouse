namespace EsWarehouse.Domain.Primitives
{
    public interface IEvent
    {
        public int Sku { get; set; }
        public int Version { get; set; }
        public string Type { get; }
    }
}