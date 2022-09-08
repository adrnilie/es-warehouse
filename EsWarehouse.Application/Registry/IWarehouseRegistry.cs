using System.Collections.Generic;
using EsWarehouse.Domain;
using EsWarehouse.Domain.Abstractions;
using EsWarehouse.Domain.Primitives;

namespace EsWarehouse.Application.Registry
{
    public interface IWarehouseRegistry
    {
        WarehouseProduct GetWarehouseProduct(int sku, IEnumerable<IEvent> events);
    }
}