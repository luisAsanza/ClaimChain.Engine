namespace ClaimsEngine.Domain.SeedWork;
public interface IDomainEvent
{
    Guid EventId { get; }
    DateTimeOffset OccurredOn { get; }
}