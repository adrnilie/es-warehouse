using System;
using System.Collections.Generic;
using System.Linq;
using EsWarehouse.Domain;
using EsWarehouse.Domain.Abstractions;
using EsWarehouse.Domain.Entities;
using EsWarehouse.Domain.Primitives;
using EsWarehouse.Infrastructure.Extensions;

namespace EsWarehouse.Infrastructure.Repositories
{
    public class WarehouseEventsRepository : IWarehouseEventsRepository
    {
        private readonly IList<EventEntity> _events = new List<EventEntity>();
        private readonly IList<Action<IEvent>> _subscribers = new List<Action<IEvent>>();

        public WarehouseProduct GetWarehouseProduct(int sku)
        {
            var warehouseProduct = new WarehouseProduct(sku);
            if (!_events.Any())
            {
                return warehouseProduct;
            }

            warehouseProduct.RestoreFrom(_events.Select(ev => ev.ToEvent()));
            return warehouseProduct;
        }

        public IEnumerable<IEvent> GetAllEvents(int sku)
        {
            return _events.Where(ev => ev.CorrelationId == sku).Select(ev => ev.ToEvent());
        }

        public void Persist(EventEntity evnt)
        {
            NotifySubscribers(() => _events.Add(evnt), evnt.ToEvent());
        }

        public void Persist(IEnumerable<EventEntity> evnts)
        {
            foreach (var @event in evnts)
            {
                NotifySubscribers(() => _events.Add(@event), @event.ToEvent());
            }
        }

        public void Persist(IEvent evnt)
        {
            NotifySubscribers(() => Persist(evnt.ToEntity()), evnt);
        }

        public void Persist(IEnumerable<IEvent> evnts)
        {
            Persist(evnts.Select(ev => ev.ToEntity()));
        }

        public void Subscribe(params Action<IEvent>[] subscribers)
        {
            if (!subscribers.Any())
            {
                return;
            }

            foreach (var subscriber in subscribers)
            {
                _subscribers.Add(subscriber);
            }
        }
        
        private void NotifySubscribers(Action action, IEvent evnt)
        {
            action.Invoke();

            if (!_subscribers.Any())
            {
                return;
            }

            foreach (var subscriber in _subscribers)
            {
                subscriber.Invoke(evnt);
            }
        }
    }
}