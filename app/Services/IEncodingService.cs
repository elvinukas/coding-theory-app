using app.Models.Encode;

namespace app.Services;
using app.Models;

public interface IEncodingService
{
    EncodeResponse Encode(EncodeRequest request);
    bool CanHandle(EncodeRequest request); // checking whether the service can handle the request
    
    
    
}