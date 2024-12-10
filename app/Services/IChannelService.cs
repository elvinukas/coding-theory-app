using app.Models;

namespace app.Services;

/// <summary>
/// Interface for implementing a channel service.
/// </summary>
public interface IChannelService
{
    /// <summary>
    /// Method to pass data through a channel.
    /// </summary>
    /// <param name="request"><see cref="ChannelRequest"/></param>
    /// <returns><see cref="ChannelResponse"/></returns>
    ChannelResponse PassThrough(ChannelRequest request);
    
    /// <summary>
    /// Checking whether the service can handle the request.
    /// </summary>
    /// <param name="request"><see cref="ChannelRequest"/></param>
    /// <returns><c>bool</c></returns>
    bool CanHandle(ChannelRequest request); 
}