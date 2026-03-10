using System;
using ClaimsEngine.Domain.Aggregates.ClaimAggregate;

namespace ClaimsEngine.Application.Features.Claims.Commands;

public sealed record PatientDto(
    string Name,
    DateTime? DateOfBirth,
    RelationshipToInsured RelationshipToInsured
);
