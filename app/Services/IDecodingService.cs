using app.Models.Decode;

namespace app.Services;

public interface IDecodingService
{
    DecodeResponse Decode(DecodeRequest request);
    bool CanHandle(DecodeRequest request); // checking whether the service can handle the request
}