using System.Collections.Generic;

namespace EsWarehouse.Domain.Primitives
{
    public interface IPersist<in TEvent> where TEvent : IEvent
    {
        void Persist(TEvent evnt);
    }

    public interface IPersistMany<in TEvent> where TEvent : IEvent
    {
        void Persist(IEnumerable<TEvent> evnts);
    }
}