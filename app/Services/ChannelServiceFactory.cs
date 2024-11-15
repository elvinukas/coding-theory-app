using app.Exceptions;
using app.Models;

namespace app.Services;

public class ChannelServiceFactory
{
    private readonly IEnumerable<IChannelService> _channelServices;

    public ChannelServiceFactory(IEnumerable<IChannelService> channelServices)
    {
        _channelServices = channelServices;
    }

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