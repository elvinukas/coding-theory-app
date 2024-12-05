using app.Controllers;
using app.Exceptions;
using app.Math;
using app.Models;

namespace app.Services;

public class ImageChannelService : IChannelService
{
    private readonly IWebHostEnvironment _environment;

    public ImageChannelService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }
    
    public bool CanHandle(ChannelRequest request) => request is ImageChannelRequest;

    public ChannelResponse PassThrough(ChannelRequest request)
    {
        try
        {
            ImageChannelRequest imageRequest = (ImageChannelRequest)request;
            string tempDirectory = Path.Combine(_environment.ContentRootPath, "temp");
            string binaryPath = Path.Combine(tempDirectory, imageRequest.FileName + "._encoded.bin");

            List<List<int>> listMatrix = imageRequest.GeneratorMatrix;
            Matrix matrix = new Matrix(MatrixConverter.ConvertToIntArray(listMatrix));
            int k = matrix.Rows;
            int n = matrix.Columns;

            double errorPercentage = imageRequest.ErrorPercentage;

            Channel channel = new Channel(binaryPath, errorPercentage, k, n);

            return new ImageChannelResponse
            {
                Message = "Image successfully channeled."
            };

        }
        catch (Exception ex)
        {
            throw new ChannelException("Image could not be channeled. Error: " + ex.Message);
        }
        
        
    }
    
}