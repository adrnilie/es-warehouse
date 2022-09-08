using System.Collections.Generic;
using System.Linq;
using EsWarehouse.Domain.Abstractions;
using EsWarehouse.Domain.Entities;
using EsWarehouse.Domain.Primitives;
using EsWarehouse.Infrastructure.Extensions;

namespace EsWarehouse.Infrastructure.Repositories
{
    public class WarehouseEventsRepository : IWarehouseEventsRepository
    {
        private readonly IList<EventEntity> _events = new List<EventEntity>();

        public IEnumerable<IEvent> GetAllEvents(int sku)
        {
            return _events.Where(ev => ev.CorrelationId == sku).Select(ev => ev.ToEvent());
        }

        public void Persist(EventEntity evnt)
        {
            _events.Add(evnt);
        }

        public void Persist(IEnumerable<EventEntity> evnts)
        {
            foreach (var @event in evnts)
            {
                _events.Add(@event);
            }
        }

        public void Persist(IEvent evnt)
        {
            Persist(evnt.ToEntity());
        }

        public void Persist(IEnumerable<IEvent> evnts)
        {
            Persist(evnts.Select(ev => ev.ToEntity()));
        }
    }
}