namespace Monads;

public record Error(string Message, string? Details = null, Exception? Exception = null);