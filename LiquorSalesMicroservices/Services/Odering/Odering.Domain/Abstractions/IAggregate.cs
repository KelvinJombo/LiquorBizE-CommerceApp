namespace Odering.Domain.Abstractions
{

    public interface IAggregate<T> : IAggregate, IEntityTypeConfiguration<T>
    {
    }
    public interface IAggregate : IEntity
    {
        IReadOnlyList<IDomainEvent> DomainEvents { get; }
        IDomainEvent[] ClearDomainEvents();
    }
}
