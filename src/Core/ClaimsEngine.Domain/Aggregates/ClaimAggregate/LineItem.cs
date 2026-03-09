namespace ClaimsEngine.Domain.Aggregates.ClaimAggregate;

    public sealed class LineItem
    {
        public Guid Id { get; private set; }
        public decimal Amount { get; private set; }
        public string Description { get; private set; }

        #region EF Core materialization constructor
        private LineItem()
        {
            // EF Core requires a parameterless constructor for materialization.
            // Compiler enforces that all properties are initialized, to avoid the warning 
            // about non-nullable properties not being initialized.
            Description = null!;
        }
        #endregion

        public LineItem(string description, decimal amount)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(description, nameof(description));
            ArgumentOutOfRangeException.ThrowIfNegative(amount, nameof(amount));

            Id = Guid.NewGuid();
            Description = description.Trim();
            Amount = amount;
        }
    }
