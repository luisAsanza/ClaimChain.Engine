using ClaimsEngine.Domain.SeedWork.Results;

namespace ClaimsEngine.Domain.SeedWork;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error? Error { get; }

    protected Result(bool isSuccess, Error? error)
    {
        if(isSuccess && error != null) 
            throw new ArgumentException("Successful result cannot have an error.");
        if(!isSuccess && error == null)
            throw new ArgumentException("Failure result must have an error.");

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, null);
    public static Result Failure(Error error) => new(false, error);
    public static Result Failure(string code, string message, ErrorType type) => new(false, new Error(code, message, type));

}
