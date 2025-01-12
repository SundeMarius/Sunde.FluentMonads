namespace Monads.Tests;

public class ResultTests
{
    [Fact]
    public void Result_Success_HasValue()
    {
        var result = Result<int>.Success(42);
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void Result_Failure_HasError()
    {
        var result = Result<int>.Failure(new Error("Something went wrong"));
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal("Something went wrong", result.Error.Message);
    }

    [Fact]
    public void Result_Failure_ThrowsWhenAccessingValue()
    {
        var result = Result<int>.Failure(new Error("Something went wrong"));
        Assert.Throws<InvalidOperationException>(() => result.Value);
    }

    [Fact]
    public void Result_Success_ThrowsWhenAccessingError()
    {
        var result = Result<int>.Success(42);
        Assert.Throws<InvalidOperationException>(() => result.Error);
    }

    [Fact]
    public void Result_Success_CanMapValue()
    {
        var result = Result<int>.Success(42);
        var mapped = result.MapSuccess(x => x * 2);
        Assert.True(mapped.IsSuccess);
        Assert.Equal(84, mapped.Value);
    }

    [Fact]
    public void Result_Failure_CanMapValue()
    {
        var result = Result<int>.Failure(new Error("Something went wrong"));
        var mapped = result.MapSuccess(x => x * 2);
        Assert.True(mapped.IsFailure);
        Assert.Equal("Something went wrong", mapped.Error.Message);
    }

    [Fact]
    public void Result_Failure_CanMapError()
    {
        var result = Result<int>.Failure(new Error("Something went wrong"));
        var mapped = result.MapError(e => new Error(e.Message + "!!!"));
        Assert.True(mapped.IsFailure);
        Assert.Equal("Something went wrong!!!", mapped.Error.Message);
    }

    [Fact]
    public void Result_Success_CanMapError()
    {
        var result = Result<int>.Success(42);
        var mapped = result.MapError(e => new Error(e.Message + "!!!"));
        Assert.True(mapped.IsSuccess);
        Assert.Equal(42, mapped.Value);
    }

    [Fact]
    public void Result_Success_Match()
    {
        var result = Result<int>.Success(42);
        var matched = result.Match(
            success => success * 2,
            failure => -1
        );
        Assert.Equal(84, matched);
    }

    [Fact]
    public void Result_Failure_Match()
    {
        var result = Result<int>.Failure(new Error("Something went wrong"));
        var matched = result.Match(
            success => success * 2,
            failure => -1
        );
        Assert.Equal(-1, matched);
    }

    [Fact]
    public void Result_Success_Or_Value()
    {
        var result = Result<int>.Success(42);
        var value = result.Or(Result<int>.Success(100));
        Assert.Equal(42, value.Value);
    }

    [Fact]
    public void Result_Failure_Or_Value()
    {
        var result = Result<int>.Failure(new Error("Something went wrong"));
        var value = result.Or(Result<int>.Success(100));
        Assert.Equal(100, value.Value);
    }

    [Fact]
    public void Result_Success_Or_Error()
    {
        var result = Result<int>.Success(42);
        var value = result.Or(Result<int>.Failure(new Error("Another error")));
        Assert.Equal(42, value.Value);
    }

    [Fact]
    public void Result_Failure_Or_Error()
    {
        var result = Result<int>.Failure(new Error("Something went wrong"));
        var value = result.Or(Result<int>.Failure(new Error("Another error")));
        Assert.Equal("Another error", value.Error.Message);
    }

    [Fact]
    public void Result_Success_OrElse_Value()
    {
        var result = Result<int>.Success(42);
        var value = result.OrElse(100);
        Assert.Equal(42, value);
    }

    [Fact]
    public void Result_Failure_OrElse_Value()
    {
        var result = Result<int>.Failure(new Error("Something went wrong"));
        var value = result.OrElse(100);
        Assert.Equal(100, value);
    }

    [Fact]
    public void Result_Success_OrElse_Func()
    {
        var result = Result<int>.Success(42);
        var value = result.OrElse(() => 100);
        Assert.Equal(42, value);
    }

    [Fact]
    public void Result_Failure_OrElse_Func()
    {
        var result = Result<int>.Failure(new Error("Something went wrong"));
        var value = result.OrElse(() => 100);
        Assert.Equal(100, value);
    }

    [Fact]
    public void Result_Success_OnSuccess()
    {
        var result = Result<int>.Success(42);
        var called = false;
        result.OnSuccess(value => called = true);
        Assert.True(called);
    }

    [Fact]
    public void Result_Failure_OnSuccess()
    {
        var result = Result<int>.Failure(new Error("Something went wrong"));
        var called = false;
        result.OnSuccess(value => called = true);
        Assert.False(called);
    }

    [Fact]
    public void Result_Success_OnFailure()
    {
        var result = Result<int>.Success(42);
        var called = false;
        result.OnFailure(error => called = true);
        Assert.False(called);
    }

    [Fact]
    public void Result_Failure_OnFailure()
    {
        var result = Result<int>.Failure(new Error("Something went wrong"));
        var called = false;
        result.OnFailure(error => called = true);
        Assert.True(called);
    }

    [Fact]
    public void Result_Success_Contains()
    {
        var result = Result<int>.Success(42);
        Assert.True(result.Contains(42));
    }

    [Fact]
    public void Result_Failure_Contains()
    {
        var result = Result<int>.Failure(new Error("Something went wrong"));
        Assert.False(result.Contains(42));
    }

    [Fact]
    public void Result_Success_IsOkAnd()
    {
        var result = Result<int>.Success(42);
        Assert.True(result.IsOkAnd(value => value == 42));
    }

    [Fact]
    public void Result_Failure_IsOkAnd()
    {
        var result = Result<int>.Failure(new Error("Something went wrong"));
        Assert.False(result.IsOkAnd(value => value == 42));
    }

    [Fact]
    public void Result_Success_IsErrAnd()
    {
        var result = Result<int>.Success(42);
        Assert.False(result.IsErrAnd(error => error.Message == "Something went wrong"));
    }

    [Fact]
    public void Result_Failure_IsErrAnd()
    {
        var result = Result<int>.Failure(new Error("Something went wrong"));
        Assert.True(result.IsErrAnd(error => error.Message == "Something went wrong"));
    }

    [Fact]
    public void Result_Success_IsErrOr()
    {
        var result = Result<int>.Success(42);
        Assert.True(result.IsErrOr(value => value == 42));
    }

    [Fact]
    public void Result_Failure_IsErrOr()
    {
        var result = Result<int>.Failure(new Error("Something went wrong"));
        Assert.True(result.IsErrOr(value => value == 42));
    }
}