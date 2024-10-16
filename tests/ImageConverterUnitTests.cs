using System.Diagnostics;
using SixLabors.ImageSharp;

namespace tests;
using app.Math;
using app.Algorithms;

// 2min 13s - 2min 30sec without any optimizations
// 41.284s after first optimizations 
// 15.693s after second optimization
// 7 - 13s after third and more optimizations
public class ImageConverterUnitTests
{
    
    // public void ImageConverter_CheckIfConverterWorksAsExpected()
    // {
    //     string imagePath = "../../../test-images/1.bmp";
    //     string binaryPath = "../../../test-images/1.bin"; 
    //     string reconvertedBinaryPath = "../../../test-images/1_reconverted.bin"; 
    //     string savePath = "../../../test-images/1_reconverted.bmp";
    //     Image image = Image.Load(imagePath);
    //     Stopwatch stopwatch = Stopwatch.StartNew();
    //     Matrix convertedImage = ImageConverter.ConvertToBinaryMatrix(image, binaryPath);
    //     stopwatch.Stop();
    //     Console.WriteLine($"ConvertToBinaryMatrix took {stopwatch.ElapsedMilliseconds} ms");
    //     stopwatch.Reset();
    //     
    //     stopwatch.Start();
    //     ImageConverter.SaveImage(convertedImage, reconvertedBinaryPath, savePath);
    //     stopwatch.Stop();
    //     Console.WriteLine($"Saving image took {stopwatch.ElapsedMilliseconds} ms");
    //
    //     byte[] convertedBinary = File.ReadAllBytes(binaryPath);
    //     byte[] reconvertedBinary = File.ReadAllBytes(reconvertedBinaryPath);
    //     Assert.True(convertedBinary.SequenceEqual(reconvertedBinary), "Converted binary file is not the same as the original one." );
    //     
    //     if (File.Exists(binaryPath))
    //     {
    //         File.Delete(binaryPath);
    //     }
    //
    //     if (File.Exists(reconvertedBinaryPath))
    //     {
    //         File.Delete(reconvertedBinaryPath);
    //     }
    //     
    //
    // }

    [Fact]
    public void ImageConverter_CheckIfTheNewConverterWorksAsExpected()
    {
        string imagePath = "../../../test-images/1.bmp";
        string binaryPath = "../../../test-images/1.bin";
        string encodedBinaryPath = "../../../test-images/1_encoded.bin";
        string decodedBinaryPath = "../../../test-images/1_decoded.bin"; 
        string savePath = "../../../test-images/1_decoded.bmp";
        Image image = Image.Load(imagePath);
        byte[] imageBytes = ImageConverter.ConvertToBinaryArray(image, binaryPath);
        
        Matrix generatorMatrix = new Matrix(new int[,]
        {
            { 1, 0, 0, 0, 1, 1, 0 },
            { 0, 1, 0, 0, 1, 0, 1 },
            { 0, 0, 1, 0, 1, 1, 1 },
            { 0, 0, 0, 1, 0, 1, 1 }
        });

        FileInfo binaryFile = new FileInfo(binaryPath);
        int originalMessageLength = (int) binaryFile.Length;
        UpdatedLinearEncodingAlgorithm.EncodeMessage(binaryPath, encodedBinaryPath, generatorMatrix);
        Channel channel = new Channel(encodedBinaryPath, 0.15, generatorMatrix.Rows, generatorMatrix.Columns);
        
        StepByStepDecodingAlgorithm algorithm = new StepByStepDecodingAlgorithm(generatorMatrix, originalMessageLength);
        algorithm.DecodeFile(encodedBinaryPath, decodedBinaryPath);
        ImageConverter.SaveImage(decodedBinaryPath, savePath);
        
    }
    
    
}