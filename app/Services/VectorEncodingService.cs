using app.Algorithms;
using app.Controllers;
using app.Exceptions;
using app.Math;
using app.Models.Encode;

namespace app.Services;

/// <summary>
/// Class implementing an encoding service for vectors.
/// </summary>
public class VectorEncodingService : IEncodingService
{
    /// <summary>
    /// Checking whether the service can handle the request.
    /// </summary>
    /// <param name="request"><see cref="EncodeRequest"/></param>
    /// <returns><c>bool</c></returns>
    public bool CanHandle(EncodeRequest request) => request is VectorEncodeRequest;

    /// <summary>
    /// Method to encode data.
    /// </summary>
    /// <param name="request"><see cref="EncodeRequest"/></param>
    /// <returns><see cref="EncodeResponse"/></returns>
    public EncodeResponse Encode(EncodeRequest request)
    {
        var vectorRequest = (VectorEncodeRequest)request;
        int[,] messageMatrixArray = MatrixConverter.ConvertToIntArray(vectorRequest.MessageMatrix);
        int[,] generatorMatrixArray = MatrixConverter.ConvertToIntArray(vectorRequest.GeneratorMatrix);
        
        Matrix messageMatrix = new Matrix(messageMatrixArray, 2);
        Matrix gMatrix = new Matrix(generatorMatrixArray, 2);
        
        try
        {
            Matrix encodedMessage = ImageLinearEncodingAlgorithm.Encode(messageMatrix, gMatrix);
            List<List<int>> encodedMessageList = MatrixConverter.ConvertTo2DList(encodedMessage);
            
            return new VectorEncodeResponse
            {
                EncodedMessage = encodedMessageList,
                Message = "Message encoded successfully.",
                Type = "vector"
            };
        }
        catch (Exception ex)
        {
            throw new EncodingException("Vector encoding failed: " + ex.Message);
        }
    }


}