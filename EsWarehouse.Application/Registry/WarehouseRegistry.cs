using System.Collections.Generic;
using System.Linq;
using EsWarehouse.Domain;
using EsWarehouse.Domain.Abstractions;
using EsWarehouse.Domain.Primitives;

namespace EsWarehouse.Application.Registry
{
    public class WarehouseRegistry : IWarehouseRegistry
    {
        public WarehouseProduct GetWarehouseProduct(int sku, IEnumerable<IEvent> events)
        {
            var warehouseProduct = new WarehouseProduct(sku);

            if (!events.Any())
            {
                return warehouseProduct;
            }

            warehouseProduct.RestoreFrom(events);
            return warehouseProduct;
        }
    }
}