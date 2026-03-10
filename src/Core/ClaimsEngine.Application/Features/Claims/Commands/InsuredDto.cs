using System;

namespace ClaimsEngine.Application.Features.Claims.Commands;

public sealed record InsuredDto(
    string Name,
    DateTime? DateOfBirth
);
