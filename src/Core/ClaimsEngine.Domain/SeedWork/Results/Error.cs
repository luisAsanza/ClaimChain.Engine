namespace ClaimsEngine.Domain.SeedWork.Results;

public sealed record Error(string Code, string Message, ErrorType Type);
