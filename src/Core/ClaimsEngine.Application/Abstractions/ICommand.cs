using MediatR;

namespace ClaimsEngine.Application.Abstractions;

public interface ICommand<out TResponse> : IRequest<TResponse>
{    
}