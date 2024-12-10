namespace app.Exceptions;

/// <summary>
/// This is an exception class for anything related to generating data.
/// </summary>
public class GeneratorException : Exception
{
    public GeneratorException() {}

    public GeneratorException(string message) : base(message) {}
    public GeneratorException(string message, Exception exception) : base(message, innerException: exception) {}
}