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
            // if file size is different, similarity cannot be calculated
            return 0.0;
        }
        
        // calculating different bytes
        int totalBytes = fileData1.Length;
        int differentBytes = 0;

        for (int i = 0; i < totalBytes; i++)
        {
            if (fileData1[i] != fileData2[i])
            {
                differentBytes++;
            }
        }

        double similarity = 1.0 - (double)differentBytes / totalBytes;
        return similarity * 100;
        
    }
    
    
    
}