using app.Exceptions;
using app.Models.Decode;

namespace app.Services;

public class DecodingServiceFactory
{
    private readonly IEnumerable<IDecodingService> _decodingServices;
    
    public DecodingServiceFactory(IEnumerable<IDecodingService> decodingServices)
    {
        _decodingServices = decodingServices;
    }

    public IDecodingService GetService(DecodeRequest request)
    {
        var service = _decodingServices.FirstOrDefault(s => s.CanHandle(request));
        if (service == null)
        {
            throw new DecodingException("No decoding service available for such request type.");
        }
        return service;
    }
    
}