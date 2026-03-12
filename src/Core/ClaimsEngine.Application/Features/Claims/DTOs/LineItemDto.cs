namespace ClaimsEngine.Application.Features.Claims.DTOs;

public sealed record LineItemDto(
    string Description,
    decimal Amount
);
