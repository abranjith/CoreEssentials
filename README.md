# CoreEssentials

A comprehensive .NET library providing essential utilities and patterns for robust application development.

## Table of Contents

- [Result](#coreessentialsresult)
  - [Overview](#overview)
  - [Key Features](#key-features)
  - [Basic Usage](#basic-usage)
  - [Advanced Usage](#advanced-usage)
  - [Extension Methods](#extension-methods)
  - [HTTP Result](#http-integration)

## Result

### Overview

`Result` API provides a robust implementation of the Result pattern that offers an elegant alternative to traditional exception-based error handling. This pattern enables you to write more predictable, composable, and maintainable code by making errors explicit in your method signatures.

The Result pattern is inspired by similar implementations in languages like Rust. Instead of throwing exceptions or returning null values, operations return a `Result<TResult, TError>` that explicitly represents either success or failure.


### Key Features

#### Core Result Operations

**Creating Results**
```csharp
// Success result
var success = Result<string, Error>.Ok("Hello, World!");

// Error result
var error = Result<string, Error>.Err(new Error("Something went wrong"));

// Implicit conversions
Result<int, string> result1 = 42;           // Success
Result<int, string> result2 = "Error";      // Error
```

**State Checking**
- `IsOk()` - Determines if the result represents success
- `IsErr()` - Determines if the result represents an error
- `Value` - Gets the success value (throws if error)
- `Error` - Gets the error value (throws if success)

**Safe Value Extraction**
- `UnwrapOr(defaultValue)` - Returns value or default if error
- `Unwrap()` - Returns value or throws exception if error

**Exception Analysis**
- `IsCausedBy<T>()` - Checks if error is caused by specific exception type
- Supports inheritance checking (derived exceptions match base types)
- Note that any Exception captured within Result object can be serialized unlike certian Exception types

#### Transformation Operations

**Value Mapping**
- `Map<TUResult>(mapFunc)` - Transform success value to new type
- `MapAsync<TUResult>(mapFunc, cancellationToken)` - Async value transformation
- `MapOr<TUResult>(mapFunc, defaultValue)` - Transform with default if error
- `MapOrAsync<TUResult>(mapFunc, defaultValue, cancellationToken)` - Async transform with default if error

**Error Mapping**
- `MapErr<TUError>(mapFunc)` - Transform error to new type
- `MapErrAsync<TUError>(mapFunc, cancellationToken)` - Async error transformation

**Conditional Mapping**
- `MapOrElse<TUResult>(okMapFunc, errMapFunc)` - Transform both success and error cases
- `MapOrElseAsync<TUResult>(okMapFunc, errMapFunc, cancellationToken)` - Async conditional mapping

#### Side Effect Operations

**Conditional Execution**
- `IsOkAnd(action)` - Execute action if success, return success status
- `IsOkAndAsync(action, cancellationToken)` - Async conditional execution for success
- `IsErrAnd(action)` - Execute action if error, return error status
- `IsErrAndAsync(action, cancellationToken)` - Async conditional execution for error

### Basic Usage

#### Simple Error Handling

```csharp
using CoreEssentials.Result;

public Result<int, string> Divide(int numerator, int denominator)
{
    if (denominator == 0)
        return "Cannot divide by zero";
    
    return numerator / denominator;
}

// Usage
var result = Divide(10, 2);
if (result.IsOk())
{
    Console.WriteLine($"Result: {result.Value}"); // Result: 5
}
else
{
    Console.WriteLine($"Error: {result.Error}");
}

// Or use safe extraction
var value = result.UnwrapOr(0); // Returns 5, or 0 if error
```

#### Exception Handling

```csharp
public Result<string, Exception> ReadFile(string path)
{
    try
    {
        var content = File.ReadAllText(path);
        return content;
    }
    catch (Exception ex)
    {
        return ex;
    }
}

// Usage with exception checking
var result = ReadFile("config.txt");
if (result.IsErr() && result.IsCausedBy<FileNotFoundException>())
{
    Console.WriteLine("Config file not found, using defaults");
}
```

### Advanced Usage

#### Chaining Operations

```csharp
public async Task<Result<UserDto, Error>> GetUserProfile(int userId)
{
    return await GetUser(userId)
        .MapAsync(user => EnrichUserData(user), cancellationToken)
        .MapAsync(enrichedUser => ConvertToDto(enrichedUser), cancellationToken);
}

// Error handling with fallbacks
public string GetDisplayName(int userId)
{
    return GetUser(userId)
        .Map(user => $"{user.FirstName} {user.LastName}")
        .MapOr(name => name.Trim(), "Anonymous User");
}
```

#### Conditional Processing

```csharp
public async Task ProcessOrder(Order order)
{
    var result = await ValidateOrder(order);
    
    // Execute side effects based on result
    await result.IsOkAndAsync(async (validOrder, ct) =>
    {
        await SendConfirmationEmail(validOrder.CustomerEmail, ct);
        await UpdateInventory(validOrder.Items, ct);
    }, cancellationToken);
    
    await result.IsErrAndAsync(async (error, ct) =>
    {
        await LogError(error, ct);
        await NotifyCustomerOfError(order.CustomerEmail, error, ct);
    }, cancellationToken);
}
```

#### Error Transformation

```csharp
public Result<ProcessedData, BusinessError> ProcessData(RawData data)
{
    return ValidateData(data)
        .MapErr(validationError => new BusinessError(
            ErrorCode.ValidationFailed, 
            validationError.Message))
        .Map(validData => TransformData(validData))
        .MapErr(processingError => new BusinessError(
            ErrorCode.ProcessingFailed, 
            processingError.Message));
}
```

All async methods support `CancellationToken` for proper cancellation handling in async scenarios.

### HTTP Result

Since HTTP calls are everywhere, this library includes HTTP extension method to convert `HttpResponseMessage` to Result object with success type & error type :

```csharp
using CoreEssentials.Http;

public async Task<Result<UserResponse, ErrorResponse>> GetUser(int id, CancellationToken ct)
{
    var response = await httpClient.GetAsync($"/api/users/{id}");
    return await response.AsResult<UserResponse, ErrorResponse>(cancellationToken: ct);
}

// Usage
var userResult = await GetUser(123);
userResult.IsOkAnd(user => Console.WriteLine($"User: {user.Name}"));
userResult.IsErrAnd(error => Console.WriteLine($"Error: {error.Message}"));
```

The `AsResult` extension automatically deserializes successful responses to the success type and error responses to the error type based on the HTTP status code. Note there is also support for custom `JsonSerializerOptions`.

---

## Installation

```bash
dotnet add package CoreEssentials
```

## Target Frameworks

- .NET Standard 2.1
- .NET 8.0+

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.
