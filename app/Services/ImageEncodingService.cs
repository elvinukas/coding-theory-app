using app.Algorithms;
using app.Controllers;
using app.Exceptions;
using app.Math;
using app.Models.Encode;
using Microsoft.AspNetCore.SignalR;
using SixLabors.ImageSharp;

namespace app.Services;

/// <summary>
/// Class implementing an encoding service for images.
/// </summary>
public class ImageEncodingService : IEncodingService
{
    private readonly IWebHostEnvironment _environment;
    private readonly IHubContext<EncodingProgressHub> _hubContext;
    
    /// <summary>
    /// Constructor for the image encoding service.
    /// </summary>
    /// <param name="environment"><see cref="IWebHostEnvironment"/></param>
    /// <param name="hubContext">Can be used to link progress of decoding to the front end</param>
    public ImageEncodingService(IWebHostEnvironment environment, IHubContext<EncodingProgressHub> hubContext)
    {
        _environment = environment;
        _hubContext = hubContext;
    }
    
    /// <summary>
    /// Checking whether the service can handle the request.
    /// </summary>
    /// <param name="request"><see cref="EncodeRequest"/></param>
    /// <returns><c>bool</c></returns>
    public bool CanHandle(EncodeRequest request) => request is ImageEncodeRequest;

    /// <summary>
    /// Method to encode data.
    /// </summary>
    /// <param name="request"><see cref="EncodeRequest"/></param>
    /// <returns><see cref="EncodeResponse"/></returns>
    /// <seealso cref="EncodeAsync"/>
    public EncodeResponse Encode(EncodeRequest request)
    {
        return EncodeAsync(request).GetAwaiter().GetResult();

    }

    /// <summary>
    /// Private method which encodes data with async.
    /// </summary>
    /// <param name="request"><see cref="EncodeRequest"/></param>
    /// <returns>Returns a task with <see cref="EncodeResponse"/></returns>
    /// <exception cref="EncodingException">Throws if encoding failed</exception>
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
            ImageLinearEncodingAlgorithm.EncodeFile(imageBinPath, encodedBinPath, gMatrix, _hubContext);


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