using System.Text;

namespace app.Algorithms;
using app.Math;

public class TextConverter : IConverter<string>
{
    public static Matrix ConvertToBinaryMatrix(string message, string path = "")
    {
        byte[] textBytes = Encoding.UTF8.GetBytes(message);
        return IConverter<string>.MakeMatrixFromByteArray(textBytes);

    }

    public static string ConvertToOriginalFormat(Matrix message, string path = "")
    {
        byte[] utf8Bytes = IConverter<string>.MakeByteArrayFromMatrix(message);
        string originalMessage = Encoding.UTF8.GetString(utf8Bytes);
        return originalMessage;
        
    }
    
    
    
    
    
}