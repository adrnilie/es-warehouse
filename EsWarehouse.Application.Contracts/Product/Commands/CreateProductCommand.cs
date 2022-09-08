namespace EsWarehouse.Application.Contracts.Product.Commands
{
    public class CreateProductCommand
    {
        public int Sku { get; set; }
        public string Name { get; set; } = default!;
        public int Quantity { get; set; }
    }
}