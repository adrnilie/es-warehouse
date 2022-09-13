using System.Collections.Generic;
using EsWarehouse.Domain.Primitives;

namespace EsWarehouse.Domain.Abstractions
{
    public interface IWarehouseEventsRepository
    {
        IEnumerable<IEvent> GetAllEvents(int sku);
        WarehouseProduct GetWarehouseProduct(int sku);
        void Persist(WarehouseProduct warehouseProduct);
    }
}