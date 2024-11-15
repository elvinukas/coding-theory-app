using app.Exceptions;
using app.Models.Encode;

namespace app.Services;

public class EncodingServiceFactory
{
    private readonly IEnumerable<IEncodingService> _encodingServices;

    public EncodingServiceFactory(IEnumerable<IEncodingService> encodingServices)
    {
        _encodingServices = encodingServices;
    }

    public IEncodingService GetService(EncodeRequest request)
    {
        var service = _encodingServices.FirstOrDefault(s => s.CanHandle(request));
        if (service == null)
        {
            throw new EncodingException("No encoding service available for such request type.");
        }
        return service;
    }
}