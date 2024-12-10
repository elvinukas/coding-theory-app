using app.Algorithms;
using app.Controllers;
using app.Exceptions;
using app.Math;
using app.Models.Decode;

namespace app.Services;

/// <summary>
/// This class is used for text decoding and implements the <see cref="IDecodingService"/>.
/// </summary>
public class TextDecodingService : IDecodingService
{
    /// <summary>
    /// Checking whether the service can handle the request.
    /// </summary>
    /// <param name="request"><see cref="DecodeRequest"/></param>
    /// <returns><c>bool</c></returns>
    public bool CanHandle(DecodeRequest request) => request is TextDecodeRequest;

    /// <summary>
    /// Method to decode data.
    /// </summary>
    /// <param name="request"><see cref="DecodeRequest"/></param>
    /// <returns><see cref="DecodeResponse"/></returns>
    public DecodeResponse Decode(DecodeRequest request)
    {
        var textRequest = (TextDecodeRequest)request;
        int[,] messageMatrixArray = MatrixConverter.ConvertToIntArray(textRequest.MessageMatrix);
        int[,] generatorMatrixArray = MatrixConverter.ConvertToIntArray(textRequest.GeneratorMatrix);
        int length = textRequest.Length;
        
        Matrix messageMatrix = new Matrix(messageMatrixArray, 2);
        Matrix gMatrix = new Matrix(generatorMatrixArray, 2);

        try
        {
            StepByStepDecodingAlgorithm algorithm = new StepByStepDecodingAlgorithm(gMatrix, length);
            Matrix decodedMessage = algorithm.DecodeMessage(messageMatrix);
            string text = TextConverter.ConvertToOriginalFormat(decodedMessage);
            
            return new TextDecodeResponse
            {
                Message = text
            };
            
        } catch (Exception ex)
        {
            throw new DecodingException("Text decoding failed: " + ex.Message);
        }
        
    }
}