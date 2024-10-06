namespace app.Exceptions;


public class DecodingException : Exception
{
    public DecodingException() {}

    public DecodingException(string message) : base(message) {}
    public DecodingException(string message, Exception exception) : base(message, innerException: exception) {}
    
}