
using System.Collections;
using SixLabors.ImageSharp.Formats.Bmp;

namespace app.Algorithms;

using System.IO;
using SixLabors.ImageSharp;


/// <summary>
/// This class converts images into other formats. Uses an external library for image processing to binary.
/// <para>Allows for checking similarity between images, since it implements <c>ISimilarity</c> interface.</para>
/// </summary>
public class ImageConverter : IConverter<Image>, ISimilarity<string>
{

    /// <summary>
    /// Converts to a binary array.
    /// </summary>
    /// <param name="image">Image object.</param>
    /// <param name="binaryFileLocation">Path to the image file.</param>
    /// <returns><c>byte[]</c></returns>
    public static byte[] ConvertToBinaryArray(Image image, string binaryFileLocation)
    {
        using (FileStream fileStream = new FileStream(binaryFileLocation, FileMode.Create))
        {
            image.Save(fileStream, new BmpEncoder());
        }
        
        return File.ReadAllBytes(binaryFileLocation);

    }

    /// <summary>
    /// Converts <c>byte[]</c> to the image format.
    /// </summary>
    /// <param name="input"><c>byte[]</c> input which will turn into a file</param>
    /// <param name="filePath">Image storage file path.</param>
    /// <returns><c>Image</c> object.</returns>
    public static Image ConvertToOriginalFormat(byte[] input, string filePath)
    {
        File.WriteAllBytes(filePath, input);

        using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            return Image.Load(fileStream);
        }
        
    }

    /// <summary>
    /// Saves an image to a specified path.
    /// </summary>
    /// <param name="pathToBinary">Path to the image binaries.</param>
    /// <param name="savePath">Save path for the image. Saves in .bmp format.</param>
    public static void SaveImage(string pathToBinary, string savePath)
    {
        byte[] byteArray = File.ReadAllBytes(pathToBinary);
        using (Image image = ImageConverter.ConvertToOriginalFormat(byteArray, savePath))
        {
            image.Save(savePath, new BmpEncoder());
        }
        
    }
    
    /// <summary>
    /// Implements similarity calculations.
    /// </summary>
    /// <param name="image1Location">path to first image</param>
    /// <param name="image2Location">path to second image</param>
    /// <returns><c>double</c> between <c>0</c> and <c>100</c>.</returns>
    public static double CalculateSimilarity(string image1Location, string image2Location)
    {
        byte[] fileData1 = File.ReadAllBytes(image1Location);
        byte[] fileData2 = File.ReadAllBytes(image2Location);

        if (fileData1.Length != fileData2.Length)
        {
            // If file sizes differ, similarity cannot be calculated
            return 0.0;
        }

        BitArray bits1 = new BitArray(fileData1);
        BitArray bits2 = new BitArray(fileData2);

        int totalBits = bits1.Count;
        int differentBits = 0;

        for (int i = 0; i < totalBits; i++)
        {
            if (bits1[i] != bits2[i])
            {
                differentBits++;
            }
        }

        double similarity = 1.0 - (double)differentBits / totalBits;
        return similarity * 100;
    }
    
}