using System;
using EsWarehouse.Domain.Entities;
using EsWarehouse.Domain.Events;
using EsWarehouse.Domain.Primitives;
using Newtonsoft.Json;

namespace EsWarehouse.Infrastructure.Extensions
{
    internal static class EventEntityExtensions
    {
        internal static EventEntity ToEntity<T>(this T evnt) where T : IEvent
            => new EventEntity
            {
                Id = Guid.NewGuid(),
                CorrelationId = evnt.Sku,
                CreationDate = DateTime.UtcNow.Date,
                Body = JsonConvert.SerializeObject(evnt),
                Version = evnt.Version,
                Type = evnt.Type
            };

        internal static IEvent ToEvent(this EventEntity evnt)
        {
            if (evnt.Type == typeof(ProductCreated).FullName)
            {
                return JsonConvert.DeserializeObject<ProductCreated>(evnt.Body);
            }

            if (evnt.Type == typeof(QuantityAdjusted).FullName)
            {
                return JsonConvert.DeserializeObject<QuantityAdjusted>(evnt.Body);
            }

            if (evnt.Type == typeof(ProductShipped).FullName)
            {
                return JsonConvert.DeserializeObject<ProductShipped>(evnt.Body);
            }

            throw new InvalidOperationException("Unsupported event");
        }
    }
}