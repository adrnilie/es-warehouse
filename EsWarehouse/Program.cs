using EsWarehouse.Application.Contracts.Product.Commands;
using EsWarehouse.Application.Registry;
using EsWarehouse.Infrastructure.Repositories;
using Newtonsoft.Json;

namespace EsWarehouse
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var warehouseEventsRepository = new WarehouseEventsRepository();
            var warehouseRegistry = new WarehouseRegistry();

            var key = string.Empty;
            while (key != "X")
            {
                Console.WriteLine("C: Create Product");
                Console.WriteLine("A: Adjust Quantity");
                Console.WriteLine("P: Print Events");

                Console.Write("> ");
                key = Console.ReadLine()?.ToUpperInvariant();
                Console.WriteLine();

                var sku = GetSkuFromConsole();
                var warehouseProductEvents = warehouseEventsRepository.GetAllEvents(sku);
                var warehouseProduct = warehouseRegistry.GetWarehouseProduct(sku, warehouseProductEvents);

                string quantityInput;
                var quantity = 0;
                switch (key)
                {
                    case "C":
                        Console.Write("Product name: ");
                        var productName = Console.ReadLine();

                        Console.Write("Quantity: ");
                        quantityInput = Console.ReadLine();

                        if (!int.TryParse(quantityInput, out quantity))
                        {
                            return;
                        }

                        warehouseProduct.CreateProduct(new CreateProductCommand
                        {
                            Name = productName,
                            Quantity = quantity
                        });

                        warehouseEventsRepository.Persist(warehouseProduct.Commit());
                        break;
                    case "A":
                        Console.Write("Quantity: ");
                        quantityInput = Console.ReadLine();

                        if (!int.TryParse(quantityInput, out quantity))
                        {
                            return;
                        }

                        warehouseProduct.AdjustQuantity(new AdjustQuantityCommand
                        {
                            Quantity = quantity
                        });

                        warehouseEventsRepository.Persist(warehouseProduct.Commit());
                        break;
                    case "P":
                        Console.WriteLine(JsonConvert.SerializeObject(warehouseProductEvents));
                        break;
                    default:
                        break;
                }
            }
        }

        private static int GetSkuFromConsole()
        {
            Console.Write("SKU: ");
            var input = Console.ReadLine();

            if (!int.TryParse(input, out var sku))
            {
                throw new InvalidOperationException("Invalid SKU number");
            }

            return sku;
        }
    }
}