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
/// </summary>
public class UpdatedLinearEncodingAlgorithm : IEncoding
{
    public static Matrix Encode(Matrix originalMessage, Matrix gMatrix)
    {
        return originalMessage * gMatrix;

    }

    public static void EncodeFile(string inputFilePath, string encodedFilePath, Matrix generatorMatrix,
        IHubContext<EncodingProgressHub>? hubContext = null)
    {
        byte[] binaryData = File.ReadAllBytes(inputFilePath);
        BitArray encodedBits = EncodeData(binaryData, generatorMatrix, hubContext);
        SaveBitsToFile(encodedBits, encodedFilePath);
    }
    
    public static Matrix Encode(Matrix originalMessage, Matrix generatorMatrix, bool divide)
    {
        if (divide)
        {
            byte[] binaryData = IConverter<TextConverter>.MakeByteArrayFromMatrix(originalMessage);
            BitArray encodedBits = EncodeData(binaryData, generatorMatrix);
            return IConverter<TextConverter>.MakeMatrixFromBitArray(encodedBits);
        }
        
        return UpdatedLinearEncodingAlgorithm.Encode(originalMessage, generatorMatrix);
        
        
    }



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
    
    
    
    private static void SaveBitsToFile(BitArray bits, string filePath)
    {
        int totalBytes = (bits.Length + 7) / 8; // calculating the amount of bytes needed
        byte[] bytes = new byte[totalBytes];
        bits.CopyTo(bytes, 0);
        File.WriteAllBytes(filePath, bytes);
    }
    
    
}