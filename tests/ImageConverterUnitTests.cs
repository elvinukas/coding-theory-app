using System.Diagnostics;
using SixLabors.ImageSharp;

namespace tests;
using app.Math;
using app.Algorithms;

// 2min 13s - 2min 30sec without any optimizations
// 41.284s after first optimizations 
// 15.693s after second optimization
// 5 - 10s after third optimization
public class ImageConverterUnitTests
{
    [Fact]
    public void ImageConverter_CheckIfConverterWorksAsExpected()
    {
        string imagePath = "../../../test-images/1.bmp";
        string binaryPath = "../../../test-images/1.bin"; 
        string expectedBinaryPath = "../../../test-images/1_expected.bin"; 
        string savePath = "../../../test-images/1_expected.bmp";
        Image image = Image.Load(imagePath);
        Stopwatch stopwatch = Stopwatch.StartNew();
        Matrix convertedImage = ImageConverter.ConvertToBinaryMatrix(image, binaryPath);
        stopwatch.Stop();
        Console.WriteLine($"ConvertToBinaryMatrix took {stopwatch.ElapsedMilliseconds} ms");
        stopwatch.Reset();
        
        stopwatch.Start();
        ImageConverter.SaveImage(convertedImage, expectedBinaryPath, savePath);
        stopwatch.Stop();
        Console.WriteLine($"Saving image took {stopwatch.ElapsedMilliseconds} ms");
        
    }
    
    
}