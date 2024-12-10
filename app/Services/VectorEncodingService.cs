using app.Algorithms;
using app.Controllers;
using app.Exceptions;
using app.Math;
using app.Models.Encode;

namespace app.Services;

public class VectorEncodingService : IEncodingService
{
    public bool CanHandle(EncodeRequest request) => request is VectorEncodeRequest;

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