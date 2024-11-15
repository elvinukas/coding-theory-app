namespace app.Exceptions;

public class GeneratorException : Exception
{
    public GeneratorException() {}

    public GeneratorException(string message) : base(message) {}
    public GeneratorException(string message, Exception exception) : base(message, innerException: exception) {}
}