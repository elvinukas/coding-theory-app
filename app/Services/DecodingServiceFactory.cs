using app.Exceptions;
using app.Models.Decode;

namespace app.Services;

/// <summary>
/// This class represents the decoding service factory, which is used to determine which <see cref="IDecodingService"/>
/// to use based on the request.
/// </summary>
public class DecodingServiceFactory
{
    private readonly IEnumerable<IDecodingService> _decodingServices;
    
    /// <summary>
    /// Constructor which receives an enumerable list of decoding services which can be used
    /// </summary>
    /// <param name="decodingServices">Enumerable generic list of classes that implement the decoding service interface</param>
    public DecodingServiceFactory(IEnumerable<IDecodingService> decodingServices)
    {
        _decodingServices = decodingServices;
    }

    /// <summary>
    /// Method which retrieves the correct <see cref="IDecodingService"/> based
    /// on the retrieved <see cref="DecodeRequest"/> child class
    /// </summary>
    /// <param name="request"><see cref="DecodeRequest"/> child object</param>
    /// <returns><see cref="IDecodingService"/></returns>
    /// <exception cref="DecodingException">Throws an exception if
    /// there is no decoding service available for the request.</exception>
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