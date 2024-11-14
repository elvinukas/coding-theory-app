namespace app.Exceptions;

public class EncodingException : Exception
{
    public EncodingException() {}

    public EncodingException(string message) : base(message) {}
    public EncodingException(string message, Exception exception) : base(message, innerException: exception) {}
}