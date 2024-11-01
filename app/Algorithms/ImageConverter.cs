
using System.Collections;
using SixLabors.ImageSharp.Formats.Bmp;

namespace app.Algorithms;

using System.IO;
using SixLabors.ImageSharp;


// using an external library for image processing to binary
public class ImageConverter : IConverter<Image>, ISimilarity<string>
{

    public static byte[] ConvertToBinaryArray(Image image, string binaryFileLocation)
    {
        using (FileStream fileStream = new FileStream(binaryFileLocation, FileMode.Create))
        {
            image.Save(fileStream, new BmpEncoder());
        }
        
        return File.ReadAllBytes(binaryFileLocation);

    }

    public static Image ConvertToOriginalFormat(byte[] input, string filePath)
    {
        File.WriteAllBytes(filePath, input);

        using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            return Image.Load(fileStream);
        }
        
    }

    public static void SaveImage(string pathToBinary, string savePath)
    {
        byte[] byteArray = File.ReadAllBytes(pathToBinary);
        using (Image image = ImageConverter.ConvertToOriginalFormat(byteArray, savePath))
        {
            image.Save(savePath, new BmpEncoder());
        }
        
    }
    
    
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