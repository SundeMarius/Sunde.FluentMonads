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

```csharp
var successResult = Result<int>.Success(42);
var failureResult = Result<int>.Failure(new Error("Something went wrong"));
```

#### Accessing the Value or Error

```csharp
if (result.IsSuccess)
{
    Console.WriteLine(result.Value);
}
else
{
    Console.WriteLine(result.Error.Message);
}
```

#### Mapping Success and Error

```csharp
var mappedSuccess = successResult.MapSuccess(x => x * 2);
var mappedError = failureResult.MapError(e => new Error(e.Message + "!!!"));
```

### Custom Error Implementation

You can create your own error types by inheriting from the `Error` class. This allows you to add additional context or properties to your errors.

#### Example

```csharp
public record DateTimeError(string Message, DateTime StartDate, DateTime EndDate) : Error(Message);
```

#### Using Custom Errors

```csharp
var dateTimeError = new DateTimeError("Date is not within the allowed range", DateTime.MinValue, DateTime.MaxValue);
var failureResult = Result<int>.Failure(dateTimeError);

if (failureResult.IsFailure)
{
    var error = (DateTimeError)failureResult.Error;
    Console.WriteLine($"Error: {error.Message}, Start Date: {error.StartDate}, End Date: {error.EndDate}");
}
```

This approach allows you to create more specific and informative error messages, improving the overall error handling in your application.

### Option

The `Option<T>` type is used to represent an optional value that can either be present (`Some`) or absent (`None`).

#### Creating an Option

```csharp
var someOption = Option<int>.Some(5);
var noneOption = Option<int>.None();
```

#### Accessing the Value

```csharp
if (option.IsSome)
{
    Console.WriteLine(option.Value);
}
else
{
    Console.WriteLine("No value");
}
```

#### Mapping Option

```csharp
var mappedOption = someOption.Map(x => x * 2);
```

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.

## Author

Marius Sunde
