using EsWarehouse.Application.Contracts.Product.Commands;
using EsWarehouse.Infrastructure.Projections;
using EsWarehouse.Infrastructure.Repositories;
using Newtonsoft.Json;

namespace EsWarehouse
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var warehouseEventsRepository = new WarehouseEventsRepository();
            var productsRepository = new ProductsRepository();
            var productProjection = new ProductProjection(productsRepository);

            warehouseEventsRepository.Subscribe(productProjection.ReceiveEvent);

            var key = string.Empty;
            while (key != "X")
            {
                Console.WriteLine();
                Console.WriteLine("C: Create Product");
                Console.WriteLine("A: Adjust Quantity");
                Console.WriteLine("S: Ship Product");
                Console.WriteLine("P: Print Events");
                Console.WriteLine("Q: Project Product");

                Console.Write("> ");
                key = Console.ReadLine()?.ToUpperInvariant();
                Console.WriteLine();

                var sku = GetSkuFromConsole();
                var warehouseProduct = warehouseEventsRepository.GetWarehouseProduct(sku);

                string? quantityInput;
                int quantity;
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
                    case "S":
                        Console.Write("Quantity: ");
                        quantityInput = Console.ReadLine();

                        if (!int.TryParse(quantityInput, out quantity))
                        {
                            return;
                        }

                        warehouseProduct.ShipProduct(new ShipProductCommand
                        {
                            Quantity = quantity
                        });

                        warehouseEventsRepository.Persist(warehouseProduct.Commit());
                        break;
                    case "P":
                        var events = warehouseEventsRepository.GetAllEvents(sku);
                        foreach (var evnt in events)
                        {
                            Console.WriteLine(JsonConvert.SerializeObject(evnt));
                        }
                        break;
                    case "Q":
                        var product = productProjection.GetProduct(sku);
                        Console.WriteLine(JsonConvert.SerializeObject(product, Formatting.Indented));
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