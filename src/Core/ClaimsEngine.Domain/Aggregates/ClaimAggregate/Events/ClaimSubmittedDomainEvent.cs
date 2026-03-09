using ClaimsEngine.Domain.SeedWork;

namespace ClaimsEngine.Domain.Aggregates.ClaimAggregate.Events;

public sealed record ClaimSubmittedDomainEvent(
        Guid ClaimId, 
        Guid CorrelationId) : DomainEvent;