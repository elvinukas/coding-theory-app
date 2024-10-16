using System.Text;

namespace app.Algorithms;
using app.Math;

// needing fixing
public class TextConverter
{
    public static Matrix ConvertToBinaryMatrix(string message, string path = "")
    {
        byte[] textBytes = Encoding.UTF8.GetBytes(message);
        return IConverter<string>.MakeMatrixFromByteArray(textBytes);

    }

    public static string ConvertToOriginalFormat(byte[] input, string path)
    {
        string originalMessage = Encoding.UTF8.GetString(input);
        return originalMessage;
    }

    public static string ConvertToOriginalFormat(Matrix message, string path = "")
    {
        byte[] utf8Bytes = IConverter<string>.MakeByteArrayFromMatrix(message);
        string originalMessage = Encoding.UTF8.GetString(utf8Bytes);
        return originalMessage;
        
    }
    
    
    
    
    
}