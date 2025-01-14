namespace Monads;

public class Option<T>
{
    public T Value
    {
        get
        {
            if (IsNone)
                throw new InvalidOperationException("Cannot access Value when the option is None.");
            return _value;
        }
    }

    public bool IsSome { get; }

    public bool IsNone => !IsSome;

    public static implicit operator Option<T>(T value) => new(value);

    public static Option<T> Some(T value) => new(value);

    public static Option<T> None() => new();

    private Option(T value)
    {
        _value = value;
        IsSome = true;
    }

    private Option()
    {
        _value = default!;
        IsSome = false;
    }

    private readonly T _value;

}

public static class OptionExtensions
{
    public static Option<Tnew> Map<T, Tnew>(this Option<T> option, Func<T, Tnew> func)
    {
        return option.IsSome ? func(option.Value) : Option<Tnew>.None();
    }

    public static void OnSome<T>(this Option<T> option, Action<T> action)
    {
        if (option.IsSome)
            action(option.Value);
    }

    public static void OnNone<T>(this Option<T> option, Action action)
    {
        if (option.IsNone)
            action();
    }

    public static Result<T> ToResult<T>(this Option<T> option, Error error)
    {
        return option.IsSome ? Result<T>.Success(option.Value) : Result<T>.Failure(error);
    }

    public static Option<T> Filter<T>(this Option<T> option, Func<T, bool> predicate)
    {
        return option.IsSome && predicate(option.Value) ? option : Option<T>.None();
    }

    public static Option<T> And<T>(this Option<T> option, Option<T> other)
    {
        return option.IsSome && other.IsSome ? other : Option<T>.None();
    }

    public static Option<T> AndThen<T>(this Option<T> option, Func<T, Option<T>> func)
    {
        return option.IsSome ? func(option.Value) : Option<T>.None();
    }


    public static Option<T> Or<T>(this Option<T> option, Option<T> other)
    {
        return option.IsSome ? option : other;
    }

    public static Option<T> OrElse<T>(this Option<T> option, T value)
    {
        return option.IsSome ? option : Option<T>.Some(value);
    }

    public static Option<T> OrElse<T>(this Option<T> option, Func<T> func)
    {
        return option.OrElse(func());
    }

    public static bool Contains<T>(this Option<T> option, T value)
    {
        return option.IsSome && option.Value!.Equals(value);
    }

    public static T Expect<T>(this Option<T> option, string message)
    {
        if (option.IsNone)
            throw new InvalidOperationException(message);
        return option.Value;
    }

    public static bool IsSomeAnd<T>(this Option<T> option, Func<T, bool> predicate)
    {
        return option.IsSome && predicate(option.Value);
    }

    public static bool IsNoneOr<T>(this Option<T> option, Func<T, bool> predicate)
    {
        return option.IsNone || predicate(option.Value);
    }
}