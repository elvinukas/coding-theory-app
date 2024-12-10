using app.Exceptions;
using app.Models.Encode;

namespace app.Services;

/// <summary>
/// This class represents the decoding service factory, which is used to determine which <see cref="IEncodingService"/>
/// to use based on the request.
/// </summary>
public class EncodingServiceFactory
{
    private readonly IEnumerable<IEncodingService> _encodingServices;

    /// <summary>
    /// Constructor which receives an enumerable list of encoding services which can be used
    /// </summary>
    /// <param name="encodingServices">Enumerable generic list of classes
    /// that implement the encoding service interface</param>
    public EncodingServiceFactory(IEnumerable<IEncodingService> encodingServices)
    {
        _encodingServices = encodingServices;
    }

    /// <summary>
    /// Method which retrieves the correct <see cref="IEncodingService"/> based
    /// on the retrieved <see cref="EncodeRequest"/> child class
    /// </summary>
    /// <param name="request"><see cref="EncodeRequest"/> child object</param>
    /// <returns><see cref="IEncodingService"/></returns>
    /// <exception cref="EncodingException">Throws an exception if
    /// there is no encoding service available for the request.</exception>
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