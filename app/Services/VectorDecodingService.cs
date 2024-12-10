using app.Algorithms;
using app.Controllers;
using app.Exceptions;
using app.Math;
using app.Models.Decode;

namespace app.Services;

/// <summary>
/// Class implementing a decoding service for vectors.
/// </summary>
public class VectorDecodingService : IDecodingService
{
    
    /// <summary>
    /// Checking whether the service can handle the request.
    /// </summary>
    /// <param name="request"><see cref="DecodeRequest"/></param>
    /// <returns><c>bool</c></returns>
    public bool CanHandle(DecodeRequest request) => request is VectorDecodeRequest;
    
    /// <summary>
    /// Method to decode data.
    /// </summary>
    /// <param name="request"><see cref="DecodeRequest"/></param>
    /// <returns><see cref="DecodeResponse"/></returns>
    public DecodeResponse Decode(DecodeRequest request)
    {
        var vectorRequest = (VectorDecodeRequest)request;
        int[,] messageMatrixArray = MatrixConverter.ConvertToIntArray(vectorRequest.MessageMatrix);
        int[,] generatorMatrixArray = MatrixConverter.ConvertToIntArray(vectorRequest.GeneratorMatrix);
        int length = vectorRequest.Length;
        
        Matrix messageMatrix = new Matrix(messageMatrixArray, 2);
        Matrix gMatrix = new Matrix(generatorMatrixArray, 2);

        try
        {
            StepByStepDecodingAlgorithm algorithm = new StepByStepDecodingAlgorithm(gMatrix, length);
            Matrix decodedMessage = algorithm.DecodeMessage(messageMatrix);
            List<List<int>> decodedMessageList = MatrixConverter.ConvertTo2DList(decodedMessage);

            return new VectorDecodeResponse
            {
                DecodedMessage = decodedMessageList,
                Message = "Message decoded successfully.",
                Type = "vector"
            };
        }
        catch (Exception ex)
        {
            throw new DecodingException("Vector decoding failed: " + ex.Message);
        }
        
    }
}