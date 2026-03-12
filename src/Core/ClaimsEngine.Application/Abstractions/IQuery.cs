using MediatR;

namespace ClaimsEngine.Application.Abstractions;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}