using System;

namespace ClaimsEngine.Application.Features.Claims.Commands;

public sealed record LineItemDto(
    string Description,
    decimal Amount
);
