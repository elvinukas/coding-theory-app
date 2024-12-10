using app.Models.Encode;

namespace app.Services;
using app.Models;


/// <summary>
/// Interface for implementing an encoding service.
/// </summary>
public interface IEncodingService
{
    
    /// <summary>
    /// Method to encode data.
    /// </summary>
    /// <param name="request"><see cref="EncodeRequest"/></param>
    /// <returns><see cref="EncodeResponse"/></returns>
    EncodeResponse Encode(EncodeRequest request);
    
    
    /// <summary>
    /// Checking whether the service can handle the request.
    /// </summary>
    /// <param name="request"><see cref="EncodeRequest"/></param>
    /// <returns><c>bool</c></returns>
    bool CanHandle(EncodeRequest request); // checking whether the service can handle the request
    
    
    
}