using System.Collections;
using System.Text;

namespace app.Algorithms;

using app.Math;

/// <summary>
/// Interface for all converter algorithms.
/// </summary>
/// <typeparam name="T">Generic type which specifies the type of converter created.</typeparam>
public interface IConverter<T>
{
    static abstract byte[] ConvertToBinaryArray(T input, string path);
    static abstract T ConvertToOriginalFormat(byte[] input, string path);
    

    /// <summary>
    /// Static method from this interface to make a matrix from a byte array. Useful while image encoding/decoding.
    /// </summary>
    /// <param name="array">byte array</param>
    /// <returns><c>Matrix</c></returns>
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

    /// <summary>
    /// Method to make a matrix from a byte file. No longer required for decoding, search for alternatives in decoding algorithms.
    /// </summary>
    /// <param name="filePath">File path from which a matrix will be made.</param>
    /// <returns><c>Matrix</c></returns>
    [Obsolete("No longer required for decoding, check decoder algorithms.")]
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
    /// <summary>
    /// Method to make a byte array from a specified matrix. Useful for image encoding/decoding.
    /// </summary>
    /// <param name="matrix">Matrix, which will be made into a byte array.</param>
    /// <returns><c>byte[]</c></returns>
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
    
    /// <summary>
    /// Makes a matrix from a bit array. Used in <see cref="UpdatedLinearEncodingAlgorithm"/> for image encoding/decoding.
    /// </summary>
    /// <param name="bitArray">Bit array.</param>
    /// <returns><c>Matrix</c></returns>
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