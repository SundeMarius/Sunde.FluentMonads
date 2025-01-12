# Monads Library

## Overview

The Monads library offers functional programming constructs for .NET, such as `Result` and `Option` types. These constructs enable more expressive and type-safe error handling and management of optional values.

## Features

- `Result<T>`: Represents a value that can either be a success or a failure, with `Error` used to represent errors with a message for error handling.
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

#### Creating a Result

You can create a `Result` object to represent either a successful operation or a failure.

```csharp
var successResult = Result<int>.Success(42);
var failureResult = Result<int>.Failure(new Error("Something went wrong"));
```

#### Mapping Success and Error

You can use the `MapSuccess` method to transform the value of a successful result, and the `MapError` method to transform the error of a failed result.

```csharp
var mappedSuccess = successResult.MapSuccess(x => x * 2);
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

The `Match` method allows you to handle both success and failure cases in a single expression.

```csharp
var result = Result<int>.Success(42);

var output = result.Match(
    success => $"Success with value: {success}",
    failure => $"Failure with error: {failure.Message}"
);

Console.WriteLine(output);
```

This will print:

```bash
Success with value: 42
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

var defaultResult = failureResult.OrElse(10);

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

##### IsOkAnd

The `IsOkAnd` method checks if the result is a success and satisfies a given predicate.

```csharp
var result = Result<int>.Success(42);

if (result.IsOkAnd(value => value > 40))
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

##### IsErrAnd

The `IsErrAnd` method checks if the result is a failure and satisfies a given predicate.

```csharp
var result = Result<int>.Failure(new Error("Something went wrong"));

if (result.IsErrAnd(error => error.Message.Contains("wrong")))
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

##### IsErrOr

The `IsErrOr` method checks if the result is a failure or satisfies a given predicate.

```csharp
var result = Result<int>.Success(42);

if (result.IsErrOr(value => value == 42))
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

### Custom Error Implementation

You can create your own error types by inheriting from the `Error` class. This allows you to add additional context or properties to your errors.

#### Example

```csharp
public record DateTimeError(DateTime StartDate, DateTime EndDate) : Error($"Date is not within the allowed range: {StartDate} - {EndDate}");
```

#### Using Custom Errors

You can use the custom error type in your `Result` objects to provide more specific error information.

```csharp
var dateTimeError = new DateTimeError(new DateTime(1912, 6, 23), new DateTime(1913, 6, 23));
var failureResult = Result<int>.Failure(dateTimeError);

if (failureResult.IsFailure)
{
    Console.WriteLine($"Error: {failureResult.Error.Message}");
}
```

In this example, if the `failureResult` is a failure, it will print:

```bash
Error: Date is not within the allowed range: 6/23/1912 12:00:00 AM - 6/23/1913 12:00:00 AM
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

The latest release of the Monads library can be found on the [GitHub Releases](https://github.com/SundeMarius/monads/releases) page.

## Code Coverage

The Monads library achieves 100% code and branch coverage for each release, ensuring high reliability and robustness.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.

## Author

Marius Sunde Sivertsen
