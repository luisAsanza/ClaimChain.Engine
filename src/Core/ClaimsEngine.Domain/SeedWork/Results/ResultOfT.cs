using ClaimsEngine.Domain.SeedWork.Results;

namespace ClaimsEngine.Domain.SeedWork;

public sealed class Result<TValue> : Result
{
    private readonly TValue _value = default!;

    private Result(TValue value) : base(true, null)
    {
        _value = value;
    }

    private Result(Error error) : base(false, error)
    {
    }

    public TValue Value
    {
        get
        {
            if (IsFailure) 
                throw new InvalidOperationException("Cannot access Value when Result is failure.");
            return _value;
        }
    }

    public static Result<TValue> Success(TValue value) => new(value);
    public new static Result<TValue> Failure(string code, string message, ErrorType type) => new(new Error(code, message, type));
    public new static Result<TValue> Failure(Error error) => new(error);

}
