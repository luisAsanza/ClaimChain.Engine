using ClaimsEngine.Application.Abstractions;
using ClaimsEngine.Domain.Aggregates.ClaimAggregate;
using MediatR;

namespace ClaimsEngine.Application.Features.Claims.Commands
{
    public sealed class SubmitClaimCommandHandler : CommandHandler<SubmitClaimCommand, Guid>
    {
        private readonly IClaimRepository _claimRepository;
        private readonly TimeProvider _timeProvider;

        public SubmitClaimCommandHandler(IClaimRepository claimRepository, TimeProvider timeProvider)
        {
            _claimRepository = claimRepository;
            _timeProvider = timeProvider;
        }

        public override async Task<Guid> Handle(SubmitClaimCommand request, CancellationToken cancellationToken)
        {
            // 1. Check if claim number already exists
            if (await _claimRepository.ExistsByClaimNumberAsync(request.ClaimNumber, cancellationToken))
            {
                throw new InvalidOperationException($"A claim with number {request.ClaimNumber} already exists.");
            }

            // 2. Map DTOs to domain Value Objects
            var patient = new Patient(request.Patient.Name, request.Patient.DateOfBirth, request.Patient.RelationshipToInsured);
            var insured = new Insured(request.Insured.Name, request.Insured.DateOfBirth);

            // 3. Create new Claim aggregate
            var claim = Claim.Create(
                request.CorrelationId,
                request.ClaimNumber,
                request.SubscriberId,
                request.PayerId,
                request.ProviderNpi,
                patient,
                insured,
                _timeProvider.GetUtcNow());

            // 4. Add line items to claim
            foreach (var lineItemDto in request.LineItems)
            {
                claim.AddLineItem(lineItemDto.Description, lineItemDto.Amount, _timeProvider.GetUtcNow());
            }

            // 5. Execute the State Transition
            claim.Submit(_timeProvider.GetUtcNow());

            // 6. Persist the Aggregate Root
            await _claimRepository.AddAsync(claim, cancellationToken);

            return claim.Id;
        }
    }
}