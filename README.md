# Monads Library

[![Coverage Status](https://coveralls.io/repos/github/SundeMarius/Monads/badge.svg?branch=main)](https://coveralls.io/github/SundeMarius/Monads?branch=main)

## Overview

The Monads library offers functional programming constructs for .NET, such as `Result` and `Option` types. These constructs enable more expressive and type-safe error handling and management of optional values.

### Benefits of Using Monads Over Exceptions

Using monads like `Result` and `Option` provides several benefits over traditional exception-based error handling:

1. **Type Safety**: Monads make error handling explicit in the type system, reducing the risk of unhandled exceptions and making the code more predictable.
2. **Composability**: Monads allow for easy composition of operations, enabling more readable and maintainable code.
3. **Immutability**: Monads encourage immutability, leading to safer and more reliable code.
4. **Separation of Concerns**: Monads separate error handling logic from business logic, making the code cleaner and easier to understand.
5. **Functional Programming**: Monads align with functional programming principles, promoting pure functions and reducing side effects.
6. **Explicit Error Handling**: Monads require explicit handling of success and failure cases, leading to more robust and error-resistant code.

### Potential Drawbacks of Using Monads

While monads offer many benefits, there are also some potential drawbacks to consider:

1. **Learning Curve**: Monads can be difficult to understand for developers who are not familiar with functional programming concepts. This can lead to a steeper learning curve and may require additional training or education.
2. **Verbosity**: Using monads can sometimes make the code more verbose, as it requires explicit handling of success and failure cases. This can lead to more boilerplate code compared to traditional exception handling.
3. **Integration with Existing Code**: Integrating monads into an existing codebase that relies heavily on exceptions can be challenging. It may require significant refactoring to adopt monads consistently throughout the codebase.
4. **C# Language Limitations**: While C# supports functional programming to some extent, it is primarily an imperative object-oriented language. This can make the use of monads feel less natural and more cumbersome compared to languages that are designed with functional programming in mind.

Despite these drawbacks, the benefits of using monads often outweigh the cons, especially in projects that prioritize type safety, composability, and functional programming principles.

## Features

- `Result<T>`: Represents a value that can either be a success or a failure, with `Error` used to represent errors with a message for error handling.
- `Result`: A non-generic version of `Result<T>` that represents the outcome of a void method, indicating success or failure without carrying a value. It is useful when you only need to signal the success or failure of an operation without returning additional data.
- `Option<T>`: Represents an optional value that can either be present (`Some`) or absent (`None`).

## Installation

To install the Monads library, add the following package reference to your project:

```xml
<PackageReference Include="Monads" />
```

Alternatively, you can use the .NET CLI to add the package:

```sh
dotnet add package Monads
```

## Usage

### Result

The `Result<T>` type is used to represent the outcome of an operation that can either succeed or fail.

The `Error` type is used in conjunction with `Result<T>` to represent errors with a message, optional details, and an optional exception. You can also implement your own custom error types by deriving from the `Error` class.

#### Creating a generic Result

You can create a `Result` object to represent either a successful operation or a failure.

```csharp
var successResult = Result<int>.Success(42);
var failureResult = Result<int>.Failure(new Error("Something went wrong"));
```

#### Mapping Success and Error

You can use the `Map` method to transform the value of a successful result, and the `MapError` method to transform the error of a failed result.

```csharp
var mappedSuccess = successResult.Map(x => x * 2);
var mappedError = failureResult.MapError(e => new Error(e.Message + "!!!"));

if (mappedSuccess.IsSuccess)
{
    Console.WriteLine(mappedSuccess.Value);
}
else
{
    Console.WriteLine(mappedSuccess.Error.Message);
}

if (mappedError.IsFailure)
{
    Console.WriteLine(mappedError.Error.Message);
}
```

For `mappedSuccess`, if the original result is a success, this will print:

```bash
84
```

For `mappedError`, if the original result is a failure, this will print:

```bash
Something went wrong!!!
```

#### Accessing the Value or Error

First, define a `Result` object:

```csharp
var result = Result<int>.Success(42);
// or
var result = Result<int>.Failure(new Error("Something went wrong"));

if (result.IsSuccess)
{
    Console.WriteLine(result.Value);
}
else
{
    Console.WriteLine(result.Error.Message);
}
```

If `result` is a success, this will print:

```bash
42
```

If `result` is a failure, this will print:

```bash
Something went wrong
```

#### Using Match

The `Match` method allows you to handle both success and failure cases.

```csharp
var result = Result<int>.Success(42);

result.Match(
    success => Console.WriteLine($"Success with value: {success}"),
    failure => Console.WriteLine($"Failure with error: {failure.Message}")
);
```

```csharp
var result = Result<int>.Failure(new Error("Something went wrong"));

result.Match(
    success => Console.WriteLine($"Success with value: {success}"),
    failure => Console.WriteLine($"Failure with error: {failure.Message}")
);
```

For a successful result, this will print:

```bash
Success with value: 42
```

For a failed result, this will print:

```bash
Failure with error: Something went wrong
```

#### Combining Results

You can use the `Or` and `OrElse` methods to combine `Result` objects.

```csharp
var successResult = Result<int>.Success(42);
var failureResult = Result<int>.Failure(new Error("Something went wrong"));

var combinedResult = failureResult.Or(successResult);

if (combinedResult.IsSuccess)
{
    Console.WriteLine(combinedResult.Value);
}
else
{
    Console.WriteLine(combinedResult.Error.Message);
}

var defaultResult = failureResult.OrElse(_ => Result<int>.Success(10));

if (defaultResult.IsSuccess)
{
    Console.WriteLine(defaultResult.Value);
}
else
{
    Console.WriteLine(defaultResult.Error.Message);
}
```

For `combinedResult`, if the original result is a failure, this will print:

```bash
42
```

For `defaultResult`, if the original result is a failure, this will print:

```bash
10
```

#### Performing Actions on Success or Failure

You can use the `OnSuccess` and `OnFailure` methods to perform actions based on whether the result is a success or a failure.

```csharp
var successResult = Result<int>.Success(42);
var failureResult = Result<int>.Failure(new Error("Something went wrong"));

successResult.OnSuccess(value => Console.WriteLine($"Success with value: {value}"));
failureResult.OnFailure(error => Console.WriteLine($"Failure with error: {error.Message}"));
```

For `successResult`, this will print:

```bash
Success with value: 42
```

For `failureResult`, this will print:

```bash
Failure with error: Something went wrong
```

#### Chaining Extension Methods

You can chain multiple extension methods to perform complex operations in a concise manner.

```csharp
var divisor = 5;
List<string> numbers = new List<string> { "8", "32", "60s", "-32", "101", "hello", "256", "-4096", "514", "1024", "dd2048" };

var results = numbers
    .Select(s =>
        ParseToInt(s)
            .Validate(IsPositive, n => new ValidationError($"{n} is not positive."))
            .Validate(IsEven, n => new ValidationError($"{n} is not even."))
            .Validate(IsPowerOfTwo, n => new ValidationError($"{n} is not a power of two."))
            .AndThen(n => Divide(n, divisor))
    ).ToList();

results.ForEach(result => result.Match(
    onSuccess: value => Console.WriteLine($"{value.Dividend} / {value.Divisor} = {value.Quotient}"),
    onFailure: error => Console.WriteLine($"Error: {error.Message}")
));

Result<Division> Divide(int dividend, int divisor)
{
    if (divisor == 0)
        return new DivisionError("Division by zero is not allowed.");
    return new Division(dividend, divisor, dividend / divisor);
}

Result<int> ParseToInt(string s)
{
    if (int.TryParse(s, out var result))
        return result;
    return new ParseError($"Failed to parse '{s}' to int.");
}

bool IsEven(int n) => n % 2 == 0;

bool IsPowerOfTwo(int n) => n != 0 && (n & (n - 1)) == 0;

bool IsPositive(int n) => n > 0;

record struct Division(int Dividend, int Divisor, int Quotient);

record ParseError(string Message) : Error(Message);

record DivisionError(string Message) : Error(Message);

record ValidationError(string Message) : Error(Message);
```

In this example, the code parses a list of strings to integers, validates them, and performs a division operation. The results are then printed, showing either the successful division or an error message.

#### Using Boolean Extension Methods for Result

The `Result` type includes several boolean extension methods that allow you to perform checks on the result.

##### Contains

The `Contains` method checks if the result contains a specific value.

```csharp
var result = Result<int>.Success(42);

if (result.Contains(42))
{
    Console.WriteLine("Result contains the value 42");
}
else
{
    Console.WriteLine("Result does not contain the value 42");
}
```

This will print:

```bash
Result contains the value 42
```

##### IsSuccessAnd

The `IsSuccessAnd` method checks if the result is a success and satisfies a given predicate.

```csharp
var result = Result<int>.Success(42);

if (result.IsSuccessAnd(value => value > 40))
{
    Console.WriteLine("Result is a success and the value is greater than 40");
}
else
{
    Console.WriteLine("Result is not a success or the value is not greater than 40");
}
```

This will print:

```bash
Result is a success and the value is greater than 40
```

##### IsFailureAnd

The `IsFailureAnd` method checks if the result is a failure and satisfies a given predicate.

```csharp
var result = Result<int>.Failure(new Error("Something went wrong"));

if (result.IsFailureAnd(error => error.Message.Contains("wrong")))
{
    Console.WriteLine("Result is a failure and the error message contains 'wrong'");
}
else
{
    Console.WriteLine("Result is not a failure or the error message does not contain 'wrong'");
}
```

This will print:

```bash
Result is a failure and the error message contains 'wrong'
```

##### IsFailureOr

The `IsFailureOr` method checks if the result is a failure or satisfies a given predicate.

```csharp
var result = Result<int>.Success(42);

if (result.IsFailureOr(value => value == 42))
{
    Console.WriteLine("Result is a failure or the value is 42");
}
else
{
    Console.WriteLine("Result is not a failure and the value is not 42");
}
```

This will print:

```bash
Result is a failure or the value is 42
```

#### Example of non generic Result

The `Result` type can be used to represent the outcome of an operation that does not return a value. Here is an example of a method that performs a division and returns a `Result` indicating success or failure.

```csharp
public Result Divide(int numerator, int denominator)
{
    if (denominator == 0)
    {
        return Result.Failure(new Error("Division by zero is not allowed"));
    }

    var result = numerator / denominator;
    Console.WriteLine($"Result of division: {result}");
    return Result.Success();
}

// Usage
var divisionResult = Divide(10, 2);

if (divisionResult.IsSuccess)
{
    Console.WriteLine("Division was successful");
}
else
{
    Console.WriteLine($"Division failed: {divisionResult.Error.Message}");
}

var failedDivisionResult = Divide(10, 0);

if (failedDivisionResult.IsFailure)
{
    Console.WriteLine($"Division failed: {failedDivisionResult.Error.Message}");
}
```

For a successful division, this will print:

```bash
Result of division: 5
Division was successful
```

For a failed division, this will print:

```bash
Division failed: Division by zero is not allowed
```

### Custom Error Implementation

You can create your own error types by inheriting from the `Error` class. This allows you to add additional context or properties to your errors.

#### Example

```csharp
public record InvalidDateRangeError(Date StartDate, Date EndDate) : Error($"Date is not within the allowed range: {StartDate.ToShortDateString()}-{EndDate.ToShortDateString()}");
```

#### Using Custom Errors

You can use the custom error type in your `Result` objects to provide more specific error information.

```csharp
var dateError = new InvalidDateRangeError(new Date(1912, 6, 23), new Date(1913, 6, 23));
var failureResult = Result<int>.Failure(dateError);

if (failureResult.IsFailure)
{
    Console.WriteLine($"Error: {failureResult.Error.Message}");
}
```

In this example, if the `failureResult` is a failure, it will print:

```bash
Error: Date is not within the allowed range: 6/23/1912-6/23/1913
```

This approach allows you to create more specific and informative error messages, improving the overall error handling in your application.

### Option

The `Option<T>` type is used to represent an optional value that can either be present (`Some`) or absent (`None`).

#### Creating an Option

You can create an `Option` object to represent either a present value (`Some`) or an absent value (`None`).

```csharp
var someOption = Option<int>.Some(5);
var noneOption = Option<int>.None();
```

#### Accessing the Value

You can access the value of an `Option` object using the `Value` property. However, you should check if the option is `Some` before accessing the value to avoid an exception.

```csharp
if (someOption.IsSome)
{
    Console.WriteLine(someOption.Value);
}
else
{
    Console.WriteLine("No value");
}

if (noneOption.IsSome)
{
    Console.WriteLine(noneOption.Value);
}
else
{
    Console.WriteLine("No value");
}
```

For `someOption`, this will print:

```bash
5
```

For `noneOption`, this will print:

```bash
No value
```

#### Contains

The `Contains` method checks if the option contains a specific value.

```csharp
var option = Option<int>.Some(5);

if (option.Contains(5))
{
    Console.WriteLine("Option contains the value 5");
}
else
{
    Console.WriteLine("Option does not contain the value 5");
}
```

This will print:

```bash
Option contains the value 5
```

#### IsSomeAnd

The `IsSomeAnd` method checks if the option is `Some` and satisfies a given predicate.

```csharp
var option = Option<int>.Some(5);

if (option.IsSomeAnd(value => value > 3))
{
    Console.WriteLine("Option is some and the value is greater than 3");
}
else
{
    Console.WriteLine("Option is none or the value is not greater than 3");
}
```

This will print:

```bash
Option is some and the value is greater than 3
```

#### IsNoneOr

The `IsNoneOr` method checks if the option is `None` or satisfies a given predicate.

```csharp
var option = Option<int>.Some(5);

if (option.IsNoneOr(value => value == 5))
{
    Console.WriteLine("Option is none or the value is 5");
}
else
{
    Console.WriteLine("Option is some and the value is not 5");
}
```

This will print:

```bash
Option is none or the value is 5
```

#### Mapping Option

You can use the `Map` method to transform the value of an `Option` object if it is `Some`.

```csharp
var mappedOption = someOption.Map(x => x * 2);

if (mappedOption.IsSome)
{
    Console.WriteLine(mappedOption.Value);
}
else
{
    Console.WriteLine("No value");
}
```

For `mappedOption`, this will print:

```bash
10
```

#### Using OnSome and OnNone

You can use the `OnSome` and `OnNone` methods to perform actions based on whether the option is `Some` or `None`.

```csharp
someOption.OnSome(value => Console.WriteLine($"Value is {value}"));
noneOption.OnNone(() => Console.WriteLine("No value present"));
```

For `someOption`, this will print:

```bash
Value is 5
```

For `noneOption`, this will print:

```bash
No value present
```

#### Filtering Option

You can use the `Filter` method to filter the value of an `Option` object based on a predicate.

```csharp
var filteredOption = someOption.Filter(x => x > 3);

if (filteredOption.IsSome)
{
    Console.WriteLine(filteredOption.Value);
}
else
{
    Console.WriteLine("No value");
}
```

For `filteredOption`, this will print:

```bash
5
```

#### Combining Options

You can use the `Or` and `OrElse` methods to combine `Option` objects.

```csharp
var combinedOption = noneOption.Or(someOption);

if (combinedOption.IsSome)
{
    Console.WriteLine(combinedOption.Value);
}
else
{
    Console.WriteLine("No value");
}

var defaultOption = noneOption.OrElse(10);

if (defaultOption.IsSome)
{
    Console.WriteLine(defaultOption.Value);
}
else
{
    Console.WriteLine("No value");
}
```

For `combinedOption`, this will print:

```bash
5
```

For `defaultOption`, this will print:

```bash
10
```

## Latest Release

The latest release of the Monads library can be found on [GitHub Releases](https://github.com/SundeMarius/monads/releases).

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.

## Author

Marius Sunde Sivertsen
