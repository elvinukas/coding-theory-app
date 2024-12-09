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
    // works only on vectors 
    static byte[] MakeByteArrayFromMatrix(Matrix matrix)
    {
        int numberOfBytes = (int)System.Math.Ceiling(matrix.Columns / 8.0);
        byte[] bytes = new byte[numberOfBytes];

        int bitPosition = 0;
        
        for (int row = 0; row < matrix.Rows; row++)
        {
            for (int col = 0; col < matrix.Columns; col++)
            {
                // calculating which bit is currently being accessed
                int byteIndex = bitPosition / 8;
                int bitInByte = 7 - (bitPosition % 8);

                
                int bitValue = matrix[row, col].Value;
                
                if (bitValue == 1)
                {
                    bytes[byteIndex] |= (byte)(1 << bitInByte);
                }
                
                bitPosition++;
            }
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