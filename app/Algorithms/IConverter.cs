using System.Text;

namespace app.Algorithms;

using app.Math;

public interface IConverter<T>
{
    static abstract Matrix ConvertToBinaryMatrix(T input, string path);
    static abstract T ConvertToOriginalFormat(Matrix input, string path);

    static Matrix MakeMatrixFromByteArray(byte[] array)
    {
        int[,] binaryVector = new int[1, array.Length * 8]; // * 8 since each byte is 8 bits long
        int column = 0;
        
        // iterating through each byte
        for (int i = 0; i < array.Length; ++i)
        {
            byte textByte = array[i];

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

    static Matrix MakeMatrixFromByteFile(string filePath)
    {
        byte[] bytes = File.ReadAllBytes(filePath);
        int[,] binaryMatrix = new int[1, bytes.Length * 8];
        Parallel.For(0, bytes.Length, i =>
        {
            byte b = bytes[i];
            for (int j = 0; j < 8; j++)
            {
                int bit = (b >> (7 - j)) & 1;
                binaryMatrix[0, i * 8 + j] = bit;
            }
        }); 

        return new Matrix(binaryMatrix);
    }

    static byte[] MakeByteArrayFromMatrix(Matrix matrix)
    {
        int numberOfBytes = matrix.Columns / 8;
        byte[] bytes = new byte[numberOfBytes];

        int column = 0;
        
        // iterating through each byte

        for (int i = 0; i < bytes.Length; ++i)
        {
            byte textByte = 0; // current byte value

            for (int bit = 7; bit >= 0; --bit)
            {
                textByte += (byte)(matrix[0, column].Value * (int)System.Math.Pow(2, bit));
                ++column;
            }

            bytes[i] = textByte;
        }

        return bytes;
    }

}