namespace EsWarehouse.Application.Contracts.Product.Commands
{
    public class AdjustQuantityCommand
    {
        public int Sku { get; set; }
        public int Quantity { get; set; }
    }
}