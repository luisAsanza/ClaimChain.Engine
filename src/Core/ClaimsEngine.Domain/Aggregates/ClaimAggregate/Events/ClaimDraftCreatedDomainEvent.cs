using ClaimsEngine.Domain.SeedWork.Abstractions;

namespace ClaimsEngine.Domain.Aggregates.ClaimAggregate.Events;

public sealed record ClaimDraftCreatedDomainEvent(
    Guid ClaimId,
    Guid CorrelationId) : DomainEvent;