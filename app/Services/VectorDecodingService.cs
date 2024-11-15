using app.Algorithms;
using app.Controllers;
using app.Exceptions;
using app.Math;
using app.Models.Decode;

namespace app.Services;

public class VectorDecodingService : IDecodingService
{
    public bool CanHandle(DecodeRequest request) => request is VectorDecodeRequest;

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
            Matrix decodedMessage = StepByStepDecodingAlgorithm.Decode(gMatrix, messageMatrix, length);
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