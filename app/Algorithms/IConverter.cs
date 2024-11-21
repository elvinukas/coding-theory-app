using System.Collections;
using System.Text;

namespace app.Algorithms;

using app.Math;

public interface IConverter<T>
{
    static abstract byte[] ConvertToBinaryArray(T input, string path);
    static abstract T ConvertToOriginalFormat(byte[] input, string path);
    

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

        for (int i = 0; i < bytes.Length; ++i)
        {
            byte b = bytes[i];
            for (int j = 0; j < 8; j++)
            {
                int bit = (b >> (7 - j)) & 1;
                binaryMatrix[0, i * 8 + j] = bit;
            }
        }

        return new Matrix(binaryMatrix);
    }

    
    // previously 1.654s
    static byte[] MakeByteArrayFromMatrix(Matrix matrix)
    {
        int numberOfBytes = matrix.Columns / 8;
        byte[] bytes = new byte[numberOfBytes];

        
        // iterating through each byte
        
        for (int i = 0; i < numberOfBytes; ++i)
        {
            byte textByte = 0; // current byte value
            int column = i * 8; // resolves race condition with multithreading

            for (int bit = 7; bit >= 0; --bit)
            {
                textByte += (byte)(matrix[0, column].Value * (int)System.Math.Pow(2, bit));
                ++column;
            }

            bytes[i] = textByte;
        }
            

        return bytes;
    }
    
    public static Matrix MakeMatrixFromBitArray(BitArray bitArray)
    {
        int[,] binaryVector = new int[1, bitArray.Length];

        for (int i = 0; i < bitArray.Length; ++i)
        {
            binaryVector[0, i] = bitArray[i] ? 1 : 0;
        }

        return new Matrix(binaryVector);
    }
    

}