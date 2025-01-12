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
}