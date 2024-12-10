namespace app.Exceptions;


/// <summary>
/// This is an exception class for anything related to decoding.
/// </summary>
public class DecodingException : Exception
{
    public DecodingException() {}

    public DecodingException(string message) : base(message) {}
    public DecodingException(string message, Exception exception) : base(message, innerException: exception) {}
    
}