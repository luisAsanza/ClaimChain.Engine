namespace ClaimsEngine.Domain.SeedWork;

public abstract record DomainEvent(Guid EventId, DateTimeOffset OccurredOn) : IDomainEvent
{
    protected DomainEvent() : this(Guid.NewGuid(), DateTimeOffset.UtcNow) { }
}