namespace app.Exceptions;

/// <summary>
/// This is an exception class for anything related to <c>Channel</c>'ing.
/// </summary>
public class ChannelException : Exception
{
    public ChannelException() {}

    public ChannelException(string message) : base(message) {}
    public ChannelException(string message, Exception exception) : base(message, innerException: exception) {}
}