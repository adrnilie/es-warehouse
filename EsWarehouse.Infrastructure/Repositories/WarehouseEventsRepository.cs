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
        private readonly IDictionary<int, ProductSnapshot> _snapshots = new Dictionary<int, ProductSnapshot>();

        private const int SnapshotInterval = 4;

        public WarehouseProduct GetWarehouseProduct(int sku)
        {
            var snapshot = GetSnapshot(sku);
            var warehouseProduct = new WarehouseProduct(sku, snapshot.State);

            var nextVersionStart = snapshot.State.CurrentVersion + 1;
            var events = _events.Where(x => x.CorrelationId == sku && x.Version >= nextVersionStart);
            if (!events.Any())
            {
                return warehouseProduct;
            }

            warehouseProduct.RestoreFrom(events.Select(ev => ev.ToEvent()));
            return warehouseProduct;
        }

        public IEnumerable<IEvent> GetAllEvents(int sku)
        {
            return _events.Where(ev => ev.CorrelationId == sku).Select(ev => ev.ToEvent());
        }

        public void Persist(WarehouseProduct warehouseProduct)
        {
            if (!warehouseProduct.GetUncommittedEvents().Any())
            {
                return;
            }

            foreach (var evnt in warehouseProduct.GetUncommittedEvents())
            {
                NotifySubscribers(() => _events.Add(evnt.ToEntity()), evnt);

                if (evnt.Version % SnapshotInterval == 0)
                {
                    SaveSnapshot(evnt.Sku, new ProductSnapshot
                    {
                        State = warehouseProduct.GetState()
                    });
                }
            }
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

        private void SaveSnapshot(int sku, ProductSnapshot snapshot)
        {
            if (!_snapshots.ContainsKey(sku))
            {
                _snapshots.Add(sku, snapshot);
            }

            _snapshots[sku] = snapshot;
        }

        private ProductSnapshot GetSnapshot(int sku)
        {
            if (!_snapshots.ContainsKey(sku))
            {
                return new ProductSnapshot();
            }

            return _snapshots[sku];
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

    public class ProductSnapshot
    {
        public WarehouseProductState State { get; set; } = new WarehouseProductState();
    }
}