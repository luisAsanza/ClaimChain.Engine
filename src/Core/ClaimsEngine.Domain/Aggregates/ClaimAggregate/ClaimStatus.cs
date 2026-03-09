namespace ClaimsEngine.Domain.Aggregates.ClaimAggregate;

public enum ClaimStatus
{
    Draft,
    Submitted,
    Approved,
    Rejected,
    Cancelled
}