using System.Collections;

namespace app.Algorithms;

public class ComparableImage : ISimilarity<ComparableImage>
{
    public string FilePath { get; set; }

    public ComparableImage(string filePath)
    {
        FilePath = filePath;
    }

    public static double CalculateSimilarity(ComparableImage image1, ComparableImage image2)
    {
        byte[] fileData1 = File.ReadAllBytes(image1.FilePath);
        byte[] fileData2 = File.ReadAllBytes(image2.FilePath);

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