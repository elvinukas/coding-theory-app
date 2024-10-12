
using SixLabors.ImageSharp.Formats.Bmp;

namespace app.Algorithms;

using app.Math;
using app.Algorithms;
using System.Drawing;
using System.IO;
using SixLabors.ImageSharp;


// using an external library for image processing to binary
public class ImageConverter : IConverter<Image>
{

    public static Matrix ConvertToBinaryMatrix(Image image, string binaryFileLocation)
    {
        using (FileStream fileStream = new FileStream(binaryFileLocation, FileMode.Create))
        {
            image.Save(fileStream, new BmpEncoder());
        }

        byte[] imageBytes = File.ReadAllBytes(binaryFileLocation);
        return IConverter<Image>.MakeMatrixFromByteArray(imageBytes);

    }

    public static Image ConvertToOriginalFormat(Matrix input, string filePath)
    {
        byte[] imageBytes = IConverter<Image>.MakeByteArrayFromMatrix(input);
        
        File.WriteAllBytes(filePath, imageBytes);

        using (FileStream memoryStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            return Image.Load(memoryStream);
        }
        
    }

    public static void SaveImage(Matrix input, string binaryFilePath, string savePath)
    {
        using (Image image = ImageConverter.ConvertToOriginalFormat(input, binaryFilePath))
        {
            image.Save(savePath, new BmpEncoder());
        }
        
    }
    
}