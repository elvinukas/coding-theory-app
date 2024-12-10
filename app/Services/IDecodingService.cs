using app.Models.Decode;

namespace app.Services;

/// <summary>
/// Interface for implementing a decoding service.
/// </summary>
public interface IDecodingService
{
    
    /// <summary>
    /// Method to decode data.
    /// </summary>
    /// <param name="request"><see cref="DecodeRequest"/></param>
    /// <returns><see cref="DecodeResponse"/></returns>
    DecodeResponse Decode(DecodeRequest request);
    
    /// <summary>
    /// Checking whether the service can handle the request.
    /// </summary>
    /// <param name="request"><see cref="DecodeRequest"/></param>
    /// <returns><c>bool</c></returns>
    bool CanHandle(DecodeRequest request); // checking whether the service can handle the request
}