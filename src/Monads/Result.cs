namespace Monads;

public class Result<T>
{
    public T Value
    {
        get
        {
            if (IsFailure)
                throw new InvalidOperationException("Cannot access Value when the result is a failure.");
            return _value;
        }
    }

    public Error Error
    {
        get
        {
            if (IsSuccess)
                throw new InvalidOperationException("Cannot access Error when the result is a success.");
            return _error;
        }
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public static implicit operator Result<T>(T value) => new(value);

    public static implicit operator Result<T>(Error error) => new(error);

    public static Result<T> Success(T value) => new(value);

    public static Result<T> Failure(Error error) => new(error);

    protected Result(T value)
    {
        _value = value;
        IsSuccess = true;
        _error = new Error(string.Empty);
    }

    protected Result(Error error)
    {
        _value = default!;
        IsSuccess = false;
        _error = error;
    }

    private readonly T _value;
    private readonly Error _error;
}

public class Result : Result<object>
{
    public static Result Success() => new(new object());

    public new static Result Failure(Error error) => new(error);

    public new Error Error
    {
        get
        {
            if (IsSuccess)
                throw new InvalidOperationException("Cannot access Error when the result is a success.");
            return base.Error;
        }
    }

    private Result(object value) : base(value) { }

    private Result(Error error) : base(error) { }

}

public static class ResultExtensions
{
    public static Result<Tnew> MapSuccess<T, Tnew>(this Result<T> result, Func<T, Tnew> func)
    {
        return result.IsSuccess ? func(result.Value) : result.Error;
    }

    public static Result<T> MapError<T>(this Result<T> result, Func<Error, Error> func)
    {
        return result.IsFailure ? func(result.Error) : result.Value;
    }

    public static T Match<T>(this Result<T> result, Func<T, T> success, Func<Error, T> failure)
    {
        return result.IsSuccess ? success(result.Value) : failure(result.Error);
    }

    public static Result<T> Or<T>(this Result<T> result, Result<T> other)
    {
        return result.IsFailure ? other : result.Value;
    }

    public static T OrElse<T>(this Result<T> result, T value)
    {
        return result.IsFailure ? value : result.Value;
    }

    public static T OrElse<T>(this Result<T> result, Func<T> func)
    {
        return result.OrElse(func());
    }

    public static Result<T> OnSuccess<T>(this Result<T> result, Action<T> action)
    {
        if (result.IsSuccess)
            action(result.Value);
        return result;
    }

    public static Result<T> OnFailure<T>(this Result<T> result, Action<Error> action)
    {
        if (result.IsFailure)
            action(result.Error);
        return result;
    }

    public static bool Contains<T>(this Result<T> result, T value)
    {
        return result.IsSuccess && EqualityComparer<T>.Default.Equals(result.Value, value);
    }

    public static bool IsOkAnd<T>(this Result<T> result, Func<T, bool> predicate)
    {
        return result.IsSuccess && predicate(result.Value);
    }

    public static bool IsErrAnd<T>(this Result<T> result, Func<Error, bool> predicate)
    {
        return result.IsFailure && predicate(result.Error);
    }

    public static bool IsErrOr<T>(this Result<T> result, Func<T, bool> predicate)
    {
        return result.IsFailure || predicate(result.Value);
    }
}
