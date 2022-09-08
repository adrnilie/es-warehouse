namespace EsWarehouse.Domain.Primitives
{
    public interface IApply<in T> where T : IEvent
    {
        void Apply(T evnt);
    }
}