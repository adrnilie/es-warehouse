namespace EsWarehouse.Application.Contracts.Product.Commands
{
    public class ShipProductCommand
    {
        public int Sku { get; set; }
        public int Quantity { get; set; }
    }
}