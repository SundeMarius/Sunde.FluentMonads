namespace FluentMonads;

public class Result<T>
{
    public T Value
    {
        get
        {
            if (IsFailure)
                throw new InvalidOperationException("Cannot access Value when the result is a failure.");
            return _value!;
        }
    }

    public Error Error
    {
        get
        {
            if (IsSuccess)
                throw new InvalidOperationException("Cannot access Error when the result is a success.");
            return _error!;
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
        _value = default;
        IsSuccess = false;
        _error = error;
    }

    protected T? _value;
    protected Error? _error;
}

public class Result : Result<object>
{
    public static Result Success() => new(new object());

    public new static Result Failure(Error error) => new(error);

    private Result(object value) : base(value) { }

    private Result(Error error) : base(error) { }
}


public static class ResultExtensions
{
    public static Result<Tnew> Map<T, Tnew>(this Result<T> result, Func<T, Tnew> func)
    {
        return result.IsSuccess ? func(result.Value) : result.Error;
    }

    public static async Task<Result<TNew>> MapAsync<T, TNew>(this Result<T> result, Func<T, Task<TNew>> func)
    {
        return result.IsSuccess ? Result<TNew>.Success(await func(result.Value)) : Result<TNew>.Failure(result.Error);
    }

    public static Result<T> MapError<T>(this Result<T> result, Func<Error, Error> func)
    {
        return result.IsFailure ? func(result.Error) : result;
    }

    public static void Match<T>(this Result<T> result, Action<T> onSuccess, Action<Error> onFailure)
    {
        if (result.IsSuccess)
            onSuccess(result.Value);
        else
            onFailure(result.Error);
    }

    public static Result<T> And<T>(this Result<T> result, Result<T> other)
    {
        return result.IsSuccess ? other : result;
    }

    public static Result<TNew> AndThen<T, TNew>(this Result<T> result, Func<T, Result<TNew>> func)
    {
        return result.IsSuccess ? func(result.Value) : result.Error;
    }

    public static Task<Result<TNew>> AndThenAsync<T, TNew>(this Result<T> result, Func<T, Task<Result<TNew>>> func)
    {
        return result.IsSuccess ? func(result.Value) : Task.FromResult(Result<TNew>.Failure(result.Error));
    }

    public static Result<T> Or<T>(this Result<T> result, Result<T> other)
    {
        return result.IsSuccess ? result : other;
    }

    public static Result<T> OrElse<T>(this Result<T> result, Func<Error, Result<T>> func)
    {
        return result.IsFailure ? func(result.Error) : result;
    }

    public static T UnwrapOr<T>(this Result<T> result, T @default)
    {
        return result.IsFailure ? @default : result.Value;
    }

    public static T UnwrapOrElse<T>(this Result<T> result, Func<T> func)
    {
        return result.IsFailure ? func() : result.Value;
    }

    public static Result<T> Validate<T>(this Result<T> result, Func<T, bool> predicate, Error error)
    {
        if (result.IsFailure)
            return result;
        return predicate(result.Value) ? result : error;
    }

    public static Result<T> Validate<T>(this Result<T> result, Func<T, bool> predicate, Func<T, Error> error_func)
    {
        if (result.IsFailure)
            return result;
        return predicate(result.Value) ? result : error_func(result.Value);
    }

    public static Result<TNew> TryCatch<T, TNew>(this Result<T> result, Func<T, TNew> func)
    {
        try
        {
            return result.Map(func);
        }
        catch (Exception ex)
        {
            return new Error(ex.Message, Exception: ex);
        }
    }

    public static Result<T> TryCatch<T>(Func<T> func)
    {
        try
        {
            return func();
        }
        catch (Exception ex)
        {
            return new Error(ex.Message, Exception: ex);
        }
    }

    public static Result TryCatch(Action func)
    {
        try
        {
            func();
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(new Error(ex.Message, Exception: ex));
        }
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

    public static Option<T> Ok<T>(this Result<T> result)
    {
        return result.IsSuccess ? result.Value : Option<T>.None();
    }

    public static bool Contains<T>(this Result<T> result, T value)
    {
        return result.IsSuccess && EqualityComparer<T>.Default.Equals(result.Value, value);
    }

    public static bool IsSuccessAnd<T>(this Result<T> result, Func<T, bool> predicate)
    {
        return result.IsSuccess && predicate(result.Value);
    }

    public static bool IsFailureAnd<T>(this Result<T> result, Func<Error, bool> predicate)
    {
        return result.IsFailure && predicate(result.Error);
    }

    public static bool IsFailureOr<T>(this Result<T> result, Func<T, bool> predicate)
    {
        return result.IsFailure || predicate(result.Value);
    }
}
