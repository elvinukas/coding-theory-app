
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace app.Algorithms;

using app.Math;
using app.Algorithms;
using System.Drawing;
using System.IO;
using SixLabors.ImageSharp;


// using an external library for image processing to binary
public class ImageConverter : IConverter<Image>
{

    public static byte[] ConvertToBinaryArray(Image image, string binaryFileLocation, bool bmp = true)
    {
        using (FileStream fileStream = new FileStream(binaryFileLocation, FileMode.Create))
        {
            if (bmp)
            {
                image.Save(fileStream, new BmpEncoder());
            }
            else
            {
                image.Save(fileStream, new JpegEncoder());
            }
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

    public static void SaveImage(string pathToBinary, string savePath, bool bmp = true)
    {
        byte[] byteArray = File.ReadAllBytes(pathToBinary);
        using (Image image = ImageConverter.ConvertToOriginalFormat(byteArray, savePath))
        {
            if (bmp)
            {
                image.Save(savePath, new BmpEncoder());
            }
            else
            {
                image.Save(savePath, new JpegEncoder());
            }
            
        }
        
    }
    
}