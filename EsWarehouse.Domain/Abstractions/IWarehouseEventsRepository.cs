using System.Collections.Generic;
using EsWarehouse.Domain.Entities;
using EsWarehouse.Domain.Primitives;

namespace EsWarehouse.Domain.Abstractions
{
    public interface IWarehouseEventsRepository
    {
        IEnumerable<IEvent> GetAllEvents(int sku);
        void Persist(EventEntity evnt);
        void Persist(IEnumerable<EventEntity> evnts);
        void Persist(IEvent evnt);
        void Persist(IEnumerable<IEvent> evnts);
    }
}