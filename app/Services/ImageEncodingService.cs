using app.Algorithms;
using app.Controllers;
using app.Exceptions;
using app.Math;
using app.Models.Encode;
using SixLabors.ImageSharp;

namespace app.Services;

public class ImageEncodingService : IEncodingService
{
    private readonly IWebHostEnvironment _environment;
    
    public ImageEncodingService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }
    
    public bool CanHandle(EncodeRequest request) => request is ImageEncodeRequest;

    public EncodeResponse Encode(EncodeRequest request)
    {
        return EncodeAsync(request).GetAwaiter().GetResult();

    }

    private async Task<EncodeResponse> EncodeAsync(EncodeRequest request)
    {
        ImageEncodeRequest imageRequest = (ImageEncodeRequest)request;

        if (imageRequest.Image == null || imageRequest.Image.Length == 0)
        {
            throw new EncodingException("No file received or file is empty.");
        }
        
        // creating local temporary files for processing the image

        string tempDirectory = Path.Combine(_environment.ContentRootPath, "temp");
        Directory.CreateDirectory(tempDirectory);

        // creating path to the image and saving it
        string imagePath = Path.Combine(tempDirectory, imageRequest.Image.FileName);
        using (var fileStream = new FileStream(imagePath, FileMode.Create))
        {
            await imageRequest.Image.CopyToAsync(fileStream);
        }
        
        
        string imageBinPath = Path.ChangeExtension(imagePath, ".bin");
        string encodedBinPath = Path.ChangeExtension(imagePath, "_encoded.bin");

        try
        {
            Image image = Image.Load(imagePath);
            ImageConverter.ConvertToBinaryArray(image, imageBinPath);
            FileInfo binaryFile = new FileInfo(imageBinPath);
            int originalMessageLength = (int)binaryFile.Length;
            Matrix gMatrix = new Matrix(MatrixConverter.ConvertToIntArray(imageRequest.Matrix));
            UpdatedLinearEncodingAlgorithm.EncodeFile(imageBinPath, encodedBinPath, gMatrix);


            return new ImageEncodeResponse
            {
                Message = "Image has been encoded successfully."
            };
        }
        catch (Exception ex)
        {
            throw new EncodingException("Image encoding failed. " + ex.Message);
        }

        
    }
    
}