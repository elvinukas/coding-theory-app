using app.Algorithms;
using app.Controllers;
using app.Exceptions;
using app.Math;
using app.Models.Decode;
using Microsoft.AspNetCore.SignalR;

namespace app.Services;

public class ImageDecodingService : IDecodingService
{

    private readonly IWebHostEnvironment _environment;
    private readonly IHubContext<DecodingProgressHub> _hubContext;
    
    public ImageDecodingService(IWebHostEnvironment environment, IHubContext<DecodingProgressHub> hubContext)
    {
        _environment = environment;
        _hubContext = hubContext;
    }
    
    public bool CanHandle(DecodeRequest request) => request is ImageDecodeRequest;

    public DecodeResponse Decode(DecodeRequest request)
    {
        ImageDecodeRequest imageRequest = (ImageDecodeRequest)request;
        
        string tempDirectory = Path.Combine(_environment.ContentRootPath, "temp");
        string originalFilePath = Path.Combine(tempDirectory, imageRequest.FileName + ".bin");
        string binaryPath = Path.Combine(tempDirectory, imageRequest.FileName + "._encoded.bin");
        string decodedFilePath = Path.Combine(tempDirectory, imageRequest.FileName + "_decoded.bin");
        string decodedImageFilePath = Path.Combine(tempDirectory, imageRequest.FileName + "_decoded.bmp");

        List<List<int>> gMatrixList = imageRequest.GeneratorMatrix;
        Matrix gMatrix = new Matrix(MatrixConverter.ConvertToIntArray(gMatrixList));
        
        FileInfo binaryFile = new FileInfo(originalFilePath); 
        int originalMessageLength = (int) binaryFile.Length;

        try
        {
            StepByStepDecodingAlgorithm algorithm = new StepByStepDecodingAlgorithm(gMatrix, originalMessageLength, _hubContext);
            algorithm.DecodeFile(binaryPath, decodedFilePath);
            ImageConverter.SaveImage(decodedFilePath, decodedImageFilePath);

            return new ImageDecodeResponse
            {
                Message = "Image file successfully decoded."
            };
        }
        catch (Exception ex)
        {
            throw new DecodingException("Image file decoding was unsuccessful. Error: " + ex.Message);
        }
        
    }
}