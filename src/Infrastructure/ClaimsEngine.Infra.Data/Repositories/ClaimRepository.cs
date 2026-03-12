using ClaimsEngine.Application.Abstractions;
using ClaimsEngine.Domain.Aggregates.ClaimAggregate;
using ClaimsEngine.Infra.Data.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClaimsEngine.Infra.Data.Repositories
{
    internal sealed class ClaimRepository : IClaimRepository
    {
        private readonly ClaimDbContext _context;

        public ClaimRepository(ClaimDbContext context)
        {
            _context = context;
        }

        public Task AddAsync(Claim claim, CancellationToken cancellationToken)
        {
            _context.Claims.Add(claim);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsByClaimNumberAsync(string claimNumber, CancellationToken cancellationToken)
        {
            return _context.Claims.AnyAsync(c => c.ClaimNumber == claimNumber, cancellationToken);
        }
    }
}
