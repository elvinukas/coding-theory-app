
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

    public static Matrix ConvertToBinaryMatrix(Image image)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            image.Save(memoryStream, new BmpEncoder());
            byte[] imageByteArray = memoryStream.ToArray();
            return IConverter<Image>.MakeMatrixFromByteArray(imageByteArray);
            
        }
        
    }

    public static Image ConvertToOriginalFormat(Matrix input)
    {
        byte[] imageBytes = IConverter<Image>.MakeByteArrayFromMatrix(input);

        using (MemoryStream memoryStream = new MemoryStream(imageBytes))
        {
            return Image.Load(memoryStream);
        }
        
    }

    public static void SaveImage(Matrix input, string savePath)
    {
        using (Image image = ImageConverter.ConvertToOriginalFormat(input))
        {
            image.Save(savePath, new BmpEncoder());
        }
        
    }
    
}