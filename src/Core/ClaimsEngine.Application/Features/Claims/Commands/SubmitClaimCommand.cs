using ClaimsEngine.Application.Features.Claims.DTOs;
using ClaimsEngine.Application.Abstractions;

namespace ClaimsEngine.Application.Features.Claims.Commands;

public record SubmitClaimCommand(
    Guid CorrelationId,
    string ClaimNumber,
    string SubscriberId,
    string PayerId,
    string ProviderNpi,
    PatientDto Patient,
    InsuredDto Insured,
    List<LineItemDto> LineItems
) : ICommand<Guid>;

