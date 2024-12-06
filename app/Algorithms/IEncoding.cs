using app.Math;
using app.Services;
using Microsoft.AspNetCore.SignalR;

namespace app.Algorithms;

public interface IEncoding
{
    public static abstract Matrix Encode(Matrix originalMessage, Matrix gMatrix);

    public static abstract void EncodeFile(string filePath, string encodedFilePath, Matrix gMatrix,
        IHubContext<EncodingProgressHub> hubContext);

}