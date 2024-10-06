using System.Text;

namespace app.Algorithms;
using app.Math;

public static class TextConverter
{
    public static Matrix ConvertToBinaryMatrix(string message)
    {
        byte[] textBytes = Encoding.UTF8.GetBytes(message);
        
        // creating a matrix vector with int[,] values
        int[,] binaryVector = new int[1, textBytes.Length * 8]; // * 8 since each byte is 8 bits long
        int column = 0;
        
        // iterating through each byte
        for (int i = 0; i < textBytes.Length; ++i)
        {
            byte textByte = textBytes[i];

            // starting from the last bit of the byte
            for (int bit = 7; bit >= 0; --bit)
            {
                int bitValue = textByte / (int)System.Math.Pow(2, bit);
                binaryVector[0, column] = bitValue % 2;
                ++column;
            }
            
        }

        Matrix binaryMatrix = new Matrix(binaryVector);

        return binaryMatrix;

    }

    public static string ConvertToString(Matrix message)
    {
        int numberOfBytes = message.Columns / 8;
        byte[] utf8Bytes = new byte[numberOfBytes];

        int column = 0;
        
        // iterating through each byte

        for (int i = 0; i < utf8Bytes.Length; ++i)
        {
            byte textByte = 0; // current byte value

            for (int bit = 7; bit >= 0; --bit)
            {
                textByte += (byte)(message[0, column].Value * (int)System.Math.Pow(2, bit));
                ++column;
            }

            utf8Bytes[i] = textByte;
        }

        string originalMessage = Encoding.UTF8.GetString(utf8Bytes);
        return originalMessage;
        
    }
    
    
    
    
    
    
}