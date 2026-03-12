namespace ClaimsEngine.Domain.SeedWork.Abstractions;
public interface IDomainEvent
{
    Guid EventId { get; }
    DateTimeOffset OccurredOn { get; }
}