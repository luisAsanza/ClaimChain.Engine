using ClaimsEngine.Domain.Aggregates.ClaimAggregate.Events;
using ClaimsEngine.Domain.SeedWork;

namespace ClaimsEngine.Domain.Aggregates.ClaimAggregate
{
    public sealed class Claim : AggregateRoot
    {
        public Guid Id { get; private set; }
        public Guid CorrelationId { get; private set; }
        public string ClaimNumber { get; private set; }
        // The unique ID on the patient's insurance card
        public string SubscriberId { get; private set; }
        // The unique ID of the insurance company (payer)
        public string PayerId { get; private set; }
        // The national provider identifier (NPI) of the healthcare provider
        public string ProviderNpi { get; private set; }
        public ClaimStatus Status { get; private set; }
        public byte[] RowVersion { get; private set; } = Array.Empty<byte>(); // For optimistic concurrency control
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset UpdatedAt { get; private set; }

        // Aggregate composition
        public Patient Patient { get; private set; }
        public Insured Insured { get; private set; }

        private readonly List<LineItem> _lineItems = new();
        public IReadOnlyList<LineItem> LineItems => _lineItems.AsReadOnly();

        #region EF Core materialization constructor        
        private Claim()
        {
            // EF Core requires a parameterless constructor for materialization.
            // REsolve conflict of non-nullable properties not being initialized by assigning default values.
            ClaimNumber = null!;
            SubscriberId = null!;
            PayerId = null!;
            ProviderNpi = null!;
            Patient = null!;
            Insured = null!;
        }
        #endregion
        
        private Claim (Guid correlationId, string claimNumber, string subscriberId, string payerId, string providerNpi, Patient patient, Insured insured)
        {
            if (string.IsNullOrWhiteSpace(claimNumber))
                throw new ArgumentException("Claim number is required", nameof(claimNumber));
            if (string.IsNullOrWhiteSpace(subscriberId))
                throw new ArgumentException("Subscriber ID is required", nameof(subscriberId));
            if (string.IsNullOrWhiteSpace(payerId))
                throw new ArgumentException("Payer ID is required", nameof(payerId));
            if (string.IsNullOrWhiteSpace(providerNpi))
                throw new ArgumentException("Provider NPI is required", nameof(providerNpi));
            if (patient == null)
                throw new ArgumentNullException(nameof(patient), "Patient information is required");
            if (insured == null)
                throw new ArgumentNullException(nameof(insured), "Insured information is required");

            Id = Guid.NewGuid();
            CorrelationId = correlationId;
            ClaimNumber = claimNumber.Trim();
            SubscriberId = subscriberId.Trim();
            PayerId = payerId.Trim();
            ProviderNpi = providerNpi.Trim();
            Status = ClaimStatus.Draft;
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
            Patient = patient;
            Insured = insured;
        }

        public static Claim Create(Guid correlationId, string claimNumber, string subscriberId, string payerId, string providerNpi, Patient patient, Insured insured)
        {
            return new Claim(correlationId, claimNumber, subscriberId, payerId, providerNpi, patient, insured);
        }

        public void AddLineItem(string description, decimal amount)
        {
            var lineItem = new LineItem(description, amount);
            _lineItems.Add(lineItem);
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public void Submit()
        {
            if (Status != ClaimStatus.Draft)
                throw new InvalidOperationException(
                    $"Cannot submit claim {Id}. Current status is {Status}, expected status is {ClaimStatus.Draft}.");

            // At least one line item is required to submit a claim
            if (!_lineItems.Any())
                throw new InvalidOperationException($"Cannot submit claim {Id}. At least one line item is required.");

            Status = ClaimStatus.Submitted;
            UpdatedAt = DateTimeOffset.UtcNow;

            // Raise domain event
            var domainEvent = new ClaimSubmittedDomainEvent(Id, CorrelationId);
            AddDomainEvent(domainEvent);
        }
    }
}
