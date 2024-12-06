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
            string binaryPath = Path.Combine(tempDirectory, imageRequest.FileName);

            int k, n;
            double errorPercentage = imageRequest.ErrorPercentage;
            if (imageRequest.GeneratorMatrix != null)
            {
                List<List<int>> listMatrix = imageRequest.GeneratorMatrix;
                Matrix matrix = new Matrix(MatrixConverter.ConvertToIntArray(listMatrix));
                k = matrix.Rows;
                n = matrix.Columns;
                
            }
            else
            {
                k = 0;
                n = 0;
            }
            

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