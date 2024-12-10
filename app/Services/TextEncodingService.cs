using app.Algorithms;
using app.Controllers;
using app.Exceptions;
using app.Math;
using app.Models.Encode;

namespace app.Services;

/// <summary>
/// This class is used for text encoding and implements the <see cref="IEncodingService"/>.
/// </summary>
public class TextEncodingService : IEncodingService
{
    /// <summary>
    /// Checking whether the service can handle the request.
    /// </summary>
    /// <param name="request"><see cref="EncodeRequest"/></param>
    /// <returns><c>bool</c></returns>
    public bool CanHandle(EncodeRequest request) => request is TextEncodeRequest;

    /// <summary>
    /// Method to encode data.
    /// </summary>
    /// <param name="request"><see cref="EncodeRequest"/></param>
    /// <returns><see cref="EncodeResponse"/></returns>
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
            LinearEncodingAlgorithm algorithm =
                new LinearEncodingAlgorithm(convertedText, gMatrix, gMatrix.Rows, gMatrix.Columns);
            Matrix encodedText = algorithm.EncodedMessage;
            List<List<int>> encodedTextList = MatrixConverter.ConvertTo2DList(encodedText);
            List<List<int>> convertedTextList = MatrixConverter.ConvertTo2DList(convertedText);

            return new TextEncodeResponse
            {
                EncodedMessage = encodedTextList,
                OriginalMessageBinaryLength = convertedText.Columns,
                OriginalMessageBinary = convertedTextList,
                Message = "Text successfully encoded."
            };
        }
        catch (Exception ex)
        {
            throw new EncodingException("Text encoding failed: " + ex.Message);
        }
        
    }
    
}