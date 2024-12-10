using app.Math;
using app.Services;
using Microsoft.AspNetCore.SignalR;

namespace app.Algorithms;

/// <summary>
/// Interface for encoding algorithms.
/// </summary>
public interface IEncoding
{
    /// <summary>
    /// Abstract method to encode a matrix with a generator matrix.
    /// </summary>
    /// <param name="originalMessage">Original message matrix.</param>
    /// <param name="gMatrix">Generator matrix</param>
    /// <returns>Encoded <c>Matrix</c></returns>
    public static abstract Matrix Encode(Matrix originalMessage, Matrix gMatrix);

    /// <summary>
    /// Abstract method to encode a file with given generator matrix.
    /// </summary>
    /// <param name="filePath">Path to a file which stores the original message.</param>
    /// <param name="encodedFilePath">Path to which the encoded message will be stored.</param>
    /// <param name="gMatrix">Generator matrix.</param>
    /// <param name="hubContext">Can be possible to hook up to a hub context for progress displaying.</param>
    public static abstract void EncodeFile(string filePath, string encodedFilePath, Matrix gMatrix,
        IHubContext<EncodingProgressHub> hubContext);

}