using ClaimsEngine.Domain.Aggregates.ClaimAggregate;

namespace ClaimsEngine.Application.Features.Claims.DTOs;

public sealed record PatientDto(
    string Name,
    DateTime? DateOfBirth,
    RelationshipToInsured RelationshipToInsured
);
