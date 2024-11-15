namespace app.Exceptions;

public class ChannelException : Exception
{
    public ChannelException() {}

    public ChannelException(string message) : base(message) {}
    public ChannelException(string message, Exception exception) : base(message, innerException: exception) {}
}