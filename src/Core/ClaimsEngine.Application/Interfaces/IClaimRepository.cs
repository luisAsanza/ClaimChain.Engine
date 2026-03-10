using ClaimsEngine.Domain.Aggregates.ClaimAggregate;

namespace ClaimsEngine.Application.Interfaces
{
    public interface IClaimRepository
    {
        Task<bool> ExistsByClaimNumberAsync(string claimNumber, CancellationToken cancellationToken);
        Task AddAsync(Claim claim, CancellationToken cancellationToken);
    }
}