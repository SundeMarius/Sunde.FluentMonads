namespace Monads.Tests;

public class OptionTests
{
    [Fact]
    public void Option_Some_HasValue()
    {
        var option = Option<int>.Some(5);
        Assert.True(option.IsSome);
        Assert.Equal(5, option.Value);
    }

    [Fact]
    public void Option_None_HasNoValue()
    {
        var option = Option<int>.None();
        Assert.True(option.IsNone);
    }

    [Fact]
    public void Option_Some_CanMapValue()
    {
        var option = Option<int>.Some(5);
        var mappedOption = option.Map(x => x * 2);
        Assert.True(mappedOption.IsSome);
        Assert.Equal(10, mappedOption.Value);
    }

    [Fact]
    public void Option_None_CanMapValue()
    {
        var option = Option<int>.None();
        var mappedOption = option.Map(x => x * 2);
        Assert.True(mappedOption.IsNone);
    }

    [Fact]
    public void Option_Some_OnSome()
    {
        var option = Option<int>.Some(5);
        var result = 0;
        option.OnSome(x => result = x);
        Assert.Equal(5, result);
    }

    [Fact]
    public void Option_None_OnNone()
    {
        var option = Option<int>.None();
        var result = 0;
        option.OnNone(() => result = -1);
        Assert.Equal(-1, result);
    }

    [Fact]
    public void Option_Some_Filter()
    {
        var option = Option<int>.Some(5);
        var filteredOption = option.Filter(x => x > 3);
        Assert.True(filteredOption.IsSome);
        Assert.Equal(5, filteredOption.Value);
    }

    [Fact]
    public void Option_None_Filter()
    {
        var option = Option<int>.Some(2);
        var filteredOption = option.Filter(x => x > 3);
        Assert.True(filteredOption.IsNone);
    }

    [Fact]
    public void Option_Some_ToResult()
    {
        var option = Option<int>.Some(5);
        var result = option.ToResult(new Error("No value"));
        Assert.True(result.IsSuccess);
        Assert.Equal(5, result.Value);
    }

    [Fact]
    public void Option_None_ToResult()
    {
        var option = Option<int>.None();
        var result = option.ToResult(new Error("No value"));
        Assert.True(result.IsFailure);
        Assert.Equal("No value", result.Error.Message);
    }

    [Fact]
    public void Option_Some_Or_Value()
    {
        var option = Option<int>.Some(5);
        var otherOption = Option<int>.Some(10);
        var resultOption = option.Or(otherOption);
        Assert.True(resultOption.IsSome);
        Assert.Equal(5, resultOption.Value);
    }

    [Fact]
    public void Option_None_Or_Value()
    {
        var option = Option<int>.None();
        var otherOption = Option<int>.Some(10);
        var resultOption = option.Or(otherOption);
        Assert.True(resultOption.IsSome);
        Assert.Equal(10, resultOption.Value);
    }

    [Fact]
    public void Option_Some_OrElse_Value()
    {
        var option = Option<int>.Some(5);
        var resultOption = option.OrElse(10);
        Assert.True(resultOption.IsSome);
        Assert.Equal(5, resultOption.Value);
    }

    [Fact]
    public void Option_None_OrElse_Value()
    {
        var option = Option<int>.None();
        var resultOption = option.OrElse(10);
        Assert.True(resultOption.IsSome);
        Assert.Equal(10, resultOption.Value);
    }

    [Fact]
    public void Option_Some_OrElse_Func()
    {
        var option = Option<int>.Some(5);
        var resultOption = option.OrElse(() => 10);
        Assert.True(resultOption.IsSome);
        Assert.Equal(5, resultOption.Value);
    }

    [Fact]
    public void Option_None_OrElse_Func()
    {
        var option = Option<int>.None();
        var resultOption = option.OrElse(() => 10);
        Assert.True(resultOption.IsSome);
        Assert.Equal(10, resultOption.Value);
    }

    [Fact]
    public void Option_None_ThrowsWhenAccessingValue()
    {
        var option = Option<int>.None();
        Assert.Throws<InvalidOperationException>(() => option.Value);
    }

    [Fact]
    public void Option_Some_Contains_Value()
    {
        var option = Option<int>.Some(5);
        Assert.True(option.Contains(5));
    }

    [Fact]
    public void Option_None_DoesNotContain_Value()
    {
        var option = Option<int>.None();
        Assert.False(option.Contains(5));
    }

    [Fact]
    public void Option_Some_Expect_Value()
    {
        var option = Option<int>.Some(5);
        Assert.Equal(5, option.Expect("Should have a value"));
    }

    [Fact]
    public void Option_None_Expect_Throws()
    {
        var option = Option<int>.None();
        Assert.Throws<InvalidOperationException>(() => option.Expect("Should have a value"));
    }

    [Fact]
    public void Option_Some_IsSomeAnd_True()
    {
        var option = Option<int>.Some(5);
        Assert.True(option.IsSomeAnd(x => x > 3));
    }

    [Fact]
    public void Option_Some_IsSomeAnd_False()
    {
        var option = Option<int>.Some(2);
        Assert.False(option.IsSomeAnd(x => x > 3));
    }

    [Fact]
    public void Option_None_IsSomeAnd_False()
    {
        var option = Option<int>.None();
        Assert.False(option.IsSomeAnd(x => x > 3));
    }

    [Fact]
    public void Option_Some_IsNoneOr_True()
    {
        var option = Option<int>.Some(2);
        Assert.True(option.IsNoneOr(x => x > 1));
    }

    [Fact]
    public void Option_Some_IsNoneOr_False()
    {
        var option = Option<int>.Some(2);
        Assert.False(option.IsNoneOr(x => x > 3));
    }

    [Fact]
    public void Option_None_IsNoneOr_True()
    {
        var option = Option<int>.None();
        Assert.True(option.IsNoneOr(x => x > 3));
    }

    [Fact]
    public void Option_Some_And_Some()
    {
        var option1 = Option<int>.Some(5);
        var option2 = Option<int>.Some(10);
        var resultOption = option1.And(option2);
        Assert.True(resultOption.IsSome);
        Assert.Equal(10, resultOption.Value);
    }

    [Fact]
    public void Option_Some_And_None()
    {
        var option1 = Option<int>.Some(5);
        var option2 = Option<int>.None();
        var resultOption = option1.And(option2);
        Assert.True(resultOption.IsNone);
    }

    [Fact]
    public void Option_None_And_Some()
    {
        var option1 = Option<int>.None();
        var option2 = Option<int>.Some(10);
        var resultOption = option1.And(option2);
        Assert.True(resultOption.IsNone);
    }

    [Fact]
    public void Option_Some_AndThen_Some()
    {
        var option = Option<int>.Some(5);
        var resultOption = option.AndThen(x => x * 2);
        Assert.True(resultOption.IsSome);
        Assert.Equal(10, resultOption.Value);
    }

    [Fact]
    public void Option_Some_AndThen_None()
    {
        var option = Option<int>.Some(5);
        var resultOption = option.AndThen(x => Option<int>.None());
        Assert.True(resultOption.IsNone);
    }

    [Fact]
    public void Option_None_AndThen()
    {
        var option = Option<int>.None();
        var resultOption = option.AndThen(x => x * 2);
        Assert.True(resultOption.IsNone);
    }
}