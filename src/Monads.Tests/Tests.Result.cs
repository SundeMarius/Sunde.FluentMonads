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
        var mapped = result.Map(x => x * 2);
        Assert.True(mapped.IsSuccess);
        Assert.Equal(84, mapped.Value);
    }

    [Fact]
    public void Result_Failure_CanMapValue()
    {
        var result = Result<int>.Failure(new Error("Something went wrong"));
        var mapped = result.Map(x => x * 2);
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
        var matched = false;
        result.Match(
            success => matched = success == 42,
            failure => matched = false
        );
        Assert.True(matched);
    }

    [Fact]
    public void Result_Failure_Match()
    {
        var result = Result<int>.Failure(new Error("Something went wrong"));
        var matched = false;
        result.Match(
            success => matched = false,
            failure => matched = failure.Message == "Something went wrong"
        );
        Assert.True(matched);
    }

    [Fact]
    public void Result_Success_And()
    {
        var result1 = Result<int>.Success(42);
        var result2 = Result<int>.Success(84);
        var andResult = result1.And(result2);
        Assert.True(andResult.IsSuccess);
        Assert.Equal(84, andResult.Value);
    }

    [Fact]
    public void Result_Failure_And()
    {
        var result1 = Result<int>.Failure(new Error("Something went wrong"));
        var result2 = Result<int>.Success(84);
        var andResult = result1.And(result2);
        Assert.True(andResult.IsFailure);
        Assert.Equal("Something went wrong", andResult.Error.Message);
    }

    [Fact]
    public void Result_Success_AndThen()
    {
        var result = Result<int>.Success(42);
        var andThenResult = result.AndThen(x => Result<int>.Success(x * 2));
        Assert.True(andThenResult.IsSuccess);
        Assert.Equal(84, andThenResult.Value);
    }

    [Fact]
    public void Result_Failure_AndThen()
    {
        var result = Result<int>.Failure(new Error("Something went wrong"));
        var andThenResult = result.AndThen(x => Result<int>.Success(x * 2));
        Assert.True(andThenResult.IsFailure);
        Assert.Equal("Something went wrong", andThenResult.Error.Message);
    }

    [Fact]
    public void Result_Success_Or()
    {
        var result1 = Result<int>.Success(42);
        var result2 = Result<int>.Success(84);
        var orResult = result1.Or(result2);
        Assert.True(orResult.IsSuccess);
        Assert.Equal(42, orResult.Value);
    }

    [Fact]
    public void Result_Failure_Or()
    {
        var result1 = Result<int>.Failure(new Error("Something went wrong"));
        var result2 = Result<int>.Success(84);
        var orResult = result1.Or(result2);
        Assert.True(orResult.IsSuccess);
        Assert.Equal(84, orResult.Value);
    }

    [Fact]
    public void Result_Success_OrElse_Value()
    {
        var result = Result<int>.Success(42);
        var orElseResult = result.UnwrapOr(84);
        Assert.Equal(42, orElseResult);
    }

    [Fact]
    public void Result_Failure_OrElse_Value()
    {
        var result = Result<int>.Failure(new Error("Something went wrong"));
        var orElseResult = result.UnwrapOr(84);
        Assert.Equal(84, orElseResult);
    }

    [Fact]
    public void Result_Success_OrElse_Func()
    {
        var result = Result<int>.Success(42);
        var orElseResult = result.UnwrapOrElse(() => 84);
        Assert.Equal(42, orElseResult);
    }

    [Fact]
    public void Result_Failure_OrElse_Func()
    {
        var result = Result<int>.Failure(new Error("Something went wrong"));
        var orElseResult = result.UnwrapOrElse(() => 84);
        Assert.Equal(84, orElseResult);
    }

    [Fact]
    public void Result_Success_OrElse_ErrorFunc()
    {
        var result = Result<int>.Success(42);
        var orElseResult = result.OrElse(error => Result<int>.Success(84));
        Assert.True(orElseResult.IsSuccess);
        Assert.Equal(42, orElseResult.Value);
    }

    [Fact]
    public void Result_Failure_OrElse_ErrorFunc()
    {
        var result = Result<int>.Failure(new Error("Something went wrong"));
        var orElseResult = result.OrElse(error => Result<int>.Success(84));
        Assert.True(orElseResult.IsSuccess);
        Assert.Equal(84, orElseResult.Value);
    }

    [Fact]
    public void Result_Success_Validate()
    {
        var result = Result<int>.Success(42);
        var validatedResult = result.Validate(x => x > 40, new Error("Validation failed"));
        Assert.True(validatedResult.IsSuccess);
        Assert.Equal(42, validatedResult.Value);
    }

    [Fact]
    public void Result_Success_ValidationFailed()
    {
        var result = Result<int>.Success(36);
        var validatedResult = result.Validate(x => x > 40, new Error("Validation failed"));
        Assert.True(validatedResult.IsFailure);
        Assert.Equal("Validation failed", validatedResult.Error.Message);
    }

    [Fact]
    public void Result_Failure_Validate()
    {
        var result = Result<int>.Failure(new Error("Something went wrong"));
        var validatedResult = result.Validate(x => x > 40, new Error("Validation failed"));
        Assert.True(validatedResult.IsFailure);
        Assert.Equal("Something went wrong", validatedResult.Error.Message);
    }

    [Fact]
    public void Result_Success_Validate_WithErrorFunc_PredicateTrue()
    {
        var result = Result<int>.Success(42);
        var validatedResult = result.Validate(x => x > 40, x => new Error($"Validation failed for value {x}"));
        Assert.True(validatedResult.IsSuccess);
        Assert.Equal(42, validatedResult.Value);
    }

    [Fact]
    public void Result_Success_Validate_WithErrorFunc_PredicateFalse()
    {
        var result = Result<int>.Success(42);
        var validatedResult = result.Validate(x => x > 50, x => new Error($"Validation failed for value {x}"));
        Assert.True(validatedResult.IsFailure);
        Assert.Equal("Validation failed for value 42", validatedResult.Error.Message);
    }

    [Fact]
    public void Result_Failure_Validate_WithErrorFunc()
    {
        var result = Result<int>.Failure(new Error("Something went wrong"));
        var validatedResult = result.Validate(x => x > 40, x => new Error($"Validation failed for value {x}"));
        Assert.True(validatedResult.IsFailure);
        Assert.Equal("Something went wrong", validatedResult.Error.Message);
    }

    [Fact]
    public void Result_TryCatch_Generic_Success()
    {
        var result = ResultExtensions.TryCatch(() => 42);
        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void Result_TryCatch_Generic_Failure()
    {
        var result = ResultExtensions.TryCatch<int>(() => throw new InvalidOperationException("Something went wrong"));
        Assert.True(result.IsFailure);
        Assert.Equal("Something went wrong", result.Error.Message);
    }

    [Fact]
    public void Result_TryCatch_NonGeneric_Success()
    {
        var result = ResultExtensions.TryCatch(() => Console.WriteLine("Hello, World!"));
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Result_TryCatch_NonGeneric_Failure()
    {
        var result = ResultExtensions.TryCatch(() => throw new InvalidOperationException("Something went wrong"));
        Assert.True(result.IsFailure);
        Assert.Equal("Something went wrong", result.Error.Message);
    }

    [Fact]
    public void Result_TryCatch_Generic_WithResult_Success()
    {
        var result = Result<int>.Success(42);
        var tryCatchResult = result.TryCatch(x => x / 2);
        Assert.True(tryCatchResult.IsSuccess);
        Assert.Equal(21, tryCatchResult.Value);
    }

    [Fact]
    public void Result_TryCatch_Generic_WithResult_Failure()
    {
        var result = Result<int>.Success(42);
        var tryCatchResult = result.TryCatch(x => x / 0);
        Assert.True(tryCatchResult.IsFailure);
        Assert.Equal("Attempted to divide by zero.", tryCatchResult.Error.Message);
    }

    [Fact]
    public void Result_TryCatch_Generic_WithResult_Failure_InitialFailure()
    {
        var result = Result<int>.Failure(new Error("Initial failure"));
        var tryCatchResult = result.TryCatch(x => x / 2);
        Assert.True(tryCatchResult.IsFailure);
        Assert.Equal("Initial failure", tryCatchResult.Error.Message);
    }

    [Fact]
    public void Result_Success_TryCatch()
    {
        var result = Result<int>.Success(42);
        var tryCatchResult = result.TryCatch(x => x / 0);
        Assert.True(tryCatchResult.IsFailure);
        Assert.Equal("Attempted to divide by zero.", tryCatchResult.Error.Message);
    }

    [Fact]
    public void Result_Failure_TryCatch()
    {
        var result = Result<int>.Failure(new Error("Something went wrong"));
        var tryCatchResult = result.TryCatch(x => x / 0);
        Assert.True(tryCatchResult.IsFailure);
        Assert.Equal("Something went wrong", tryCatchResult.Error.Message);
    }

    [Fact]
    public void Result_TryCatch_Func()
    {
        var tryCatchResult = ResultExtensions.TryCatch(() => throw new Exception("Something went wrong"));
        Assert.True(tryCatchResult.IsFailure);
        Assert.Equal("Something went wrong", tryCatchResult.Error.Message);
    }

    [Fact]
    public void Result_TryCatch_Action()
    {
        var tryCatchResult = ResultExtensions.TryCatch(() => { throw new InvalidOperationException("Something went wrong"); });
        Assert.True(tryCatchResult.IsFailure);
        Assert.Equal("Something went wrong", tryCatchResult.Error.Message);
    }

    [Fact]
    public void Result_Success_OnSuccess()
    {
        var result = Result<int>.Success(42);
        var successCalled = false;
        result.OnSuccess(x => successCalled = x == 42);
        Assert.True(successCalled);
    }

    [Fact]
    public void Result_Failure_OnSuccess()
    {
        var result = Result<int>.Failure(new Error("Something went wrong"));
        var successCalled = false;
        result.OnSuccess(x => successCalled = true);
        Assert.False(successCalled);
    }

    [Fact]
    public void Result_Success_OnFailure()
    {
        var result = Result<int>.Success(42);
        var failureCalled = false;
        result.OnFailure(e => failureCalled = true);
        Assert.False(failureCalled);
    }

    [Fact]
    public void Result_Failure_OnFailure()
    {
        var result = Result<int>.Failure(new Error("Something went wrong"));
        var failureCalled = false;
        result.OnFailure(e => failureCalled = e.Message == "Something went wrong");
        Assert.True(failureCalled);
    }

    [Fact]
    public void Result_Success_Ok()
    {
        var result = Result<int>.Success(42);
        var option = result.Ok();
        Assert.True(option.IsSome);
        Assert.Equal(42, option.Value);
    }

    [Fact]
    public void Result_Failure_Ok()
    {
        var result = Result<int>.Failure(new Error("Something went wrong"));
        var option = result.Ok();
        Assert.True(option.IsNone);
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
    public void Result_Success_IsSuccessAnd()
    {
        var result = Result<int>.Success(42);
        Assert.True(result.IsSuccessAnd(x => x == 42));
    }

    [Fact]
    public void Result_Failure_IsSuccessAnd()
    {
        var result = Result<int>.Failure(new Error("Something went wrong"));
        Assert.False(result.IsSuccessAnd(x => x == 42));
    }

    [Fact]
    public void Result_Success_IsFailureAnd()
    {
        var result = Result<int>.Success(42);
        Assert.False(result.IsFailureAnd(e => e.Message == "Something went wrong"));
    }

    [Fact]
    public void Result_Failure_IsFailureAnd()
    {
        var result = Result<int>.Failure(new Error("Something went wrong"));
        Assert.True(result.IsFailureAnd(e => e.Message == "Something went wrong"));
    }

    [Fact]
    public void Result_Success_IsFailureOr()
    {
        var result = Result<int>.Success(42);
        Assert.True(result.IsFailureOr(x => x == 42));
    }

    [Fact]
    public void Result_Failure_IsFailureOr()
    {
        var result = Result<int>.Failure(new Error("Something went wrong"));
        Assert.True(result.IsFailureOr(x => x == 42));
    }

    [Fact]
    public void Result_Success_ImplicitConversion()
    {
        Result<int> result = 42;
        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void Result_Failure_ImplicitConversion()
    {
        Result<int> result = new Error("Something went wrong");
        Assert.True(result.IsFailure);
        Assert.Equal("Something went wrong", result.Error.Message);
    }

    [Fact]
    public void Result_Success_StaticSuccess()
    {
        var result = Result<int>.Success(42);
        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void Result_Failure_StaticFailure()
    {
        var result = Result<int>.Failure(new Error("Something went wrong"));
        Assert.True(result.IsFailure);
        Assert.Equal("Something went wrong", result.Error.Message);
    }

    [Fact]
    public void Result_Success_GenericSuccess()
    {
        var result = Result.Success();
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Result_Failure_GenericFailure()
    {
        var result = Result.Failure(new Error("Something went wrong"));
        Assert.True(result.IsFailure);
        Assert.Equal("Something went wrong", result.Error.Message);
    }
}