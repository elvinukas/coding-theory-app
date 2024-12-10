using System.Text;

namespace app.Algorithms;
using app.Math;

/// <summary>
/// Converter that converts strings to many other types. Implements <see cref="ISimilarity{T}"/> for comparing texts.
/// </summary>
public class TextConverter : ISimilarity<string>
{
    /// <summary>
    /// Converting a string message to a binary matrix. Uses UTF-8 encoding.
    /// </summary>
    /// <param name="message">Message that is to be converted.</param>
    /// <returns><c>Matrix</c></returns>
    public static Matrix ConvertToBinaryMatrix(string message)
    {
        byte[] textBytes = Encoding.UTF8.GetBytes(message);
        return IConverter<string>.MakeMatrixFromByteArray(textBytes);
    }

    /// <summary>
    /// Converts <c>byte[]</c> into <c>string</c>.
    /// </summary>
    /// <param name="input">Byte array input</param>
    /// <returns><c>string</c></returns>
    public static string ConvertToOriginalFormat(byte[] input)
    {
        string originalMessage = Encoding.UTF8.GetString(input);
        return originalMessage;
    }

    /// <summary>
    /// Converting a <c>Matrix</c> back to a string. Uses UTF-8 decoding.
    /// </summary>
    /// <param name="message"><c>Matrix</c> that will be turned into a string</param>
    /// <returns><c>string</c> converted message</returns>
    public static string ConvertToOriginalFormat(Matrix message)
    {
        byte[] utf8Bytes = IConverter<string>.MakeByteArrayFromMatrix(message);
        string originalMessage = Encoding.UTF8.GetString(utf8Bytes);
        return originalMessage;
        
    }

    /// <summary>
    /// Implementation of <see cref="ISimilarity{T}"/> interface. Calculates similarity between two strings.
    /// </summary>
    /// <param name="a">first string</param>
    /// <param name="b">second string</param>
    /// <returns><c>double</c></returns>
    public static double CalculateSimilarity(string a, string b)
    {
        if (a.Length != b.Length)
        {
            return 0.0;
        }

        int length = a.Length;
        int differenceCounter = 0;

        int i = 0;
        foreach (char ca in a)
        {
            if (b[i] != ca)
            {
                ++differenceCounter;
            }

            ++i;
        }

        double similarity = 1.0 - (double)differenceCounter / length;
        return similarity * 100;
    }
    
    
    
    
    
}