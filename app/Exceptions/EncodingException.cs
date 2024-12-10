namespace app.Exceptions;

/// <summary>
/// This is an exception class for anything related to encoding.
/// </summary>
public class EncodingException : Exception
{
    public EncodingException() {}

    public EncodingException(string message) : base(message) {}
    public EncodingException(string message, Exception exception) : base(message, innerException: exception) {}
}