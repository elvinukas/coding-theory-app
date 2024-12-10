using app.Exceptions;
using app.Models;

namespace app.Services;

/// <summary>
/// This class represents the channel service factory, which picks out the required services based on the request.
/// </summary>
public class ChannelServiceFactory
{
    private readonly IEnumerable<IChannelService> _channelServices;

    /// <summary>
    /// Constructor which receives an enumerable list of channel services which can be used
    /// </summary>
    /// <param name="channelServices">Enumerable generic list of classes that implement the channel service interface</param>
    public ChannelServiceFactory(IEnumerable<IChannelService> channelServices)
    {
        _channelServices = channelServices;
    }

    /// <summary>
    /// Method which retrieves the correct <see cref="IChannelService"/> based
    /// on the retrieved <see cref="ChannelRequest"/> child class
    /// </summary>
    /// <param name="request"><see cref="ChannelRequest"/> child object</param>
    /// <returns><see cref="IChannelService"/></returns>
    /// <exception cref="ChannelException">Throws an exception if
    /// there is no channel service available for the request.</exception>
    public IChannelService GetService(ChannelRequest request)
    {
        var service = _channelServices.FirstOrDefault(s => s.CanHandle(request));
        if (service == null)
        {
            throw new ChannelException("No channel service available for such request type.");
        }
        return service;
    }
    
    
}