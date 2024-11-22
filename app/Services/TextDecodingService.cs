using app.Algorithms;
using app.Controllers;
using app.Exceptions;
using app.Math;
using app.Models.Decode;

namespace app.Services;

public class TextDecodingService : IDecodingService
{
    public bool CanHandle(DecodeRequest request) => request is TextDecodeRequest;

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