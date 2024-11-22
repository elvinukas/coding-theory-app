using app.Algorithms;
using app.Controllers;
using app.Exceptions;
using app.Math;
using app.Models.Encode;

namespace app.Services;

public class TextEncodingService : IEncodingService
{
    public bool CanHandle(EncodeRequest request) => request is TextEncodeRequest;

    public EncodeResponse Encode(EncodeRequest request)
    {
        TextEncodeRequest textRequest = (TextEncodeRequest)request;
        string text = textRequest.Text;

        Matrix convertedText = TextConverter.ConvertToBinaryMatrix(text);
        // 1 0 0 0 0 1 1 0 0 1 0 0 0 1 1 0 1 1 0 0 0 1 1 0 
        // 
        Matrix gMatrix = new Matrix(MatrixConverter.ConvertToIntArray(textRequest.GeneratorMatrix));

        try
        {
            Matrix encodedText = UpdatedLinearEncodingAlgorithm.Encode(convertedText, gMatrix, true);
            List<List<int>> encodedTextList = MatrixConverter.ConvertTo2DList(encodedText);

            return new TextEncodeResponse
            {
                EncodedMessage = encodedTextList,
                OriginalMessageBinaryLength = convertedText.Columns,
                Message = "Text successfully encoded."
            };
        }
        catch (Exception ex)
        {
            throw new EncodingException("Text encoding failed: " + ex.Message);
        }
        
    }
    
}