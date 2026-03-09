namespace ClaimsEngine.Domain.Aggregates.ClaimAggregate;

public sealed record Patient
{
    public string Name { get; init; }
    public DateTime? DateOfBirth { get; init; }
    public RelationshipToInsured RelationshipToInsured { get; init; }

    public Patient(string name, DateTime? dateOfBirth, RelationshipToInsured relationshipToInsured)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required", nameof(name));

        Name = name;
        DateOfBirth = dateOfBirth;
        RelationshipToInsured = relationshipToInsured;
    }

    public Patient() {
        // EF Core requires a parameterless constructor for materialization.
        // Compiler enforces that all properties are initialized, to avoid the warning 
        // about non-nullable properties not being initialized.
        Name = null!;
    }
}
