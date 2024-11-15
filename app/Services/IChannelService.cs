using app.Models;

namespace app.Services;

public interface IChannelService
{
    ChannelResponse PassThrough(ChannelRequest request);
    bool CanHandle(ChannelRequest request); // checking whether the service can handle the request
}