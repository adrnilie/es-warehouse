using System;
using System.Collections.Generic;
using System.Linq;
using EsWarehouse.Application.Contracts.Product.Commands;
using EsWarehouse.Domain.Events;
using EsWarehouse.Domain.Primitives;

namespace EsWarehouse.Domain
{
    public class WarehouseProductState
    {
        public int Quantity { get; set; }
        public int CurrentVersion { get; set; }
    }

    public class WarehouseProduct : IAggregateRoot,
        IApply<ProductCreated>,
        IApply<QuantityAdjusted>,
        IApply<ProductShipped>
    {
        public int Sku { get; }

        private readonly IList<IEvent> _pendingEvents = new List<IEvent>();
        private readonly WarehouseProductState _currentState = new WarehouseProductState();

        public WarehouseProduct(int sku, WarehouseProductState state)
        {
            Sku = sku;
            _currentState = new WarehouseProductState
            {
                CurrentVersion = state.CurrentVersion,
                Quantity = state.Quantity
            };
        }

        public void RestoreFrom(IEnumerable<IEvent> events)
        {
            if (!events.Any())
            {
                return;
            }

            var orderedEvents = events
                .OrderByDescending(x => x.Version);

            _currentState.CurrentVersion = orderedEvents.First().Version;

            foreach (var @event in events)
            {
                Apply(@event);
            }
        }

        public void Emit(IEvent evnt)
        {
            Apply(evnt);

            evnt.Version = ++_currentState.CurrentVersion;
            _pendingEvents.Add(evnt);
        }

        public void CreateProduct(CreateProductCommand productCommand)
        {
            Emit(Adapt(productCommand));
        }

        public void AdjustQuantity(AdjustQuantityCommand adjustQuantityCommand)
        {
            Emit(Adapt(adjustQuantityCommand));
        }

        public void ShipProduct(ShipProductCommand shipProductCommand)
        {
            Emit(Adapt(shipProductCommand));
        }

        public void Apply(IEvent evnt)
        {
            switch (evnt)
            {
                case ProductCreated productCreated:
                    Apply(productCreated);
                    break;
                case QuantityAdjusted quantityAdjusted:
                    Apply(quantityAdjusted);
                    break;
                case ProductShipped productShipped:
                    Apply(productShipped);
                    break;
                default:
                    throw new InvalidOperationException("Unsupported event");
            }
        }
        
        public void Apply(ProductCreated evnt)
        {
            _currentState.Quantity += evnt.Quantity;
        }
        public void Apply(QuantityAdjusted evnt)
        {
            _currentState.Quantity += evnt.Quantity;
        }
        
        public void Apply(ProductShipped evnt)
        {
            _currentState.Quantity -= evnt.Quantity;
        }

        public IEnumerable<IEvent> GetUncommittedEvents()
        {
            return _pendingEvents;
        }

        public WarehouseProductState GetState()
        {
            return _currentState;
        }

        private ProductCreated Adapt(CreateProductCommand command)
            => new ProductCreated
            {
                Sku = Sku,
                Name = command.Name,
                Quantity = command.Quantity
            };

        private QuantityAdjusted Adapt(AdjustQuantityCommand command)
            => new QuantityAdjusted
            {
                Sku = Sku,
                Quantity = command.Quantity
            };

        private ProductShipped Adapt(ShipProductCommand command)
            => new ProductShipped
            {
                Sku = Sku,
                Quantity = command.Quantity
            };
    }
}