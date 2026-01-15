namespace TaskFlow.SharedKernel.Domain;

public abstract class AggregateRoot<TEntityId> : Entity<TEntityId>
{
    private readonly List<IDomainEvent> _domainEvents = [];

    protected AggregateRoot()
    {
    }

    protected AggregateRoot(TEntityId id) : base(id)
    {
    }
    
    public ICollection<IDomainEvent> GetDomainEvents => _domainEvents;

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}