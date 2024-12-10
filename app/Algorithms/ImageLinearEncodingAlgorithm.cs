using System.Collections;
using System.Runtime.CompilerServices;
using app.Services;
using Microsoft.AspNetCore.SignalR;

namespace app.Algorithms;
using app.Math;

// warning! the encoding algorithm works with only binary digits!
// since appending the length of the message is in binary, a matrix with different field elements would be created

// the length of the original message is considered to be industry-based (known) information, which is not needed
// to be sent through the original vector

/// <summary>
/// This class uses static methods to encode binary vectors using a linear encoding algorithm.
/// <para><b>Should only be used for image encoding!
/// Use <see cref="LinearEncodingAlgorithm"/> for other types of encoding. </b></para>
/// <para>This class incorporates more methods specificly designed for image encoding. </para>
/// </summary>
public class ImageLinearEncodingAlgorithm : IEncoding
{
    /// <summary>
    /// Static method which encodes an original message by multiplying it and the generator matrix.
    /// </summary>
    /// <param name="originalMessage">original message</param>
    /// <param name="gMatrix">generator <c>Matrix</c></param>
    /// <returns><c>Matrix</c></returns>
    public static Matrix Encode(Matrix originalMessage, Matrix gMatrix)
    {
        return originalMessage * gMatrix;

    }

    /// <summary>
    /// Static method designed for encoding files.
    /// </summary>
    /// <param name="inputFilePath">The file path where to-be-encoded binary data is stored.</param>
    /// <param name="encodedFilePath">The output file path where the encoded binary data is to be stored.</param>
    /// <param name="generatorMatrix">Generator <c>Matrix</c></param>
    /// <param name="hubContext">Optional argument that can be used to output the progress of encoding.</param>
    public static void EncodeFile(string inputFilePath, string encodedFilePath, Matrix generatorMatrix,
        IHubContext<EncodingProgressHub>? hubContext = null)
    {
        byte[] binaryData = File.ReadAllBytes(inputFilePath);
        BitArray encodedBits = EncodeData(binaryData, generatorMatrix, hubContext);
        SaveBitsToFile(encodedBits, encodedFilePath);
    }
    
    /// <summary>
    /// Private helper method to encode data that has been converted to <c>byte[]</c>.
    /// </summary>
    /// <param name="binaryData">file data converted to <c>byte[]</c></param>
    /// <param name="generatorMatrix">generator <c>Matrix</c></param>
    /// <param name="hubContext">Optional argument that can be used to output the progress of encoding.</param>
    /// <returns></returns>
    private static BitArray EncodeData(byte[] binaryData, Matrix generatorMatrix,
        IHubContext<EncodingProgressHub>? hubContext = null)
    {
        int k = generatorMatrix.Rows;
        int n = generatorMatrix.Columns;

        // add padding if necessary
        BitArray tempBitArray = new BitArray(binaryData);
        int totalBits = tempBitArray.Count;
        int numberOfParts = (totalBits + k - 1) / k;
        int neededBits = numberOfParts * k;

        BitArray bitArray = new BitArray(neededBits);
    
        for (int i = 0; i < totalBits; i++)
        {
            bitArray[i] = tempBitArray[i];
        }

        // adding the padding
        for (int i = totalBits; i < neededBits; i++)
        {
            bitArray[i] = false; // padding with 0s
        }
    
        // encoding
        BitArray encodedBits = new BitArray(numberOfParts * n);
    
        int counter = 0;
        object lockObject = new object();

        Parallel.For(0, numberOfParts, part =>
        {
            int[,] messagePartArray = new int[1, k];
            for (int i = 0; i < k; i++)
            {
                messagePartArray[0, i] = bitArray[part * k + i] ? 1 : 0; // convert bit to int
            }

            Matrix partMatrix = new Matrix(messagePartArray);
            Matrix encodedPartMatrix = Encode(partMatrix, generatorMatrix);

            lock (lockObject)
            {
                // storing the encoded bits into a array
                for (int column = 0; column < n; ++column)
                {
                    encodedBits[part * n + column] = encodedPartMatrix[0, column].Value == 1; // the boolean value is automatically converted to a bit 1 or 0
                }
                        
                ++counter;
                if (part % 500000 == 0 || part == numberOfParts)
                {
                    Console.WriteLine("Message part " + counter + "/" + numberOfParts + " encoded.");
                    hubContext?.Clients.All.SendAsync("ReceiveEncodeProgress", counter, numberOfParts);
                }
            }
            
            
            
        });

        return encodedBits;
    }
    
    
    /// <summary>
    /// Method to save bits to a specified file.
    /// </summary>
    /// <param name="bits"><c>BitArray</c> which is to be stored.</param>
    /// <param name="filePath">File path where bits will be stored.</param>
    private static void SaveBitsToFile(BitArray bits, string filePath)
    {
        int totalBytes = (bits.Length + 7) / 8; // calculating the amount of bytes needed
        byte[] bytes = new byte[totalBytes];
        bits.CopyTo(bytes, 0);
        File.WriteAllBytes(filePath, bytes);
    }
    
    
}