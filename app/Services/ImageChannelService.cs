using app.Controllers;
using app.Exceptions;
using app.Math;
using app.Models;

namespace app.Services;

/// <summary>
/// Class implementing a channel service for images.
/// </summary>
public class ImageChannelService : IChannelService
{
    private readonly IWebHostEnvironment _environment;

    /// <summary>
    /// Constructor for the image channel service.
    /// </summary>
    /// <param name="environment"><see cref="IWebHostEnvironment"/></param>
    public ImageChannelService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }
    
    /// <summary>
    /// Checking whether the service can handle the request.
    /// </summary>
    /// <param name="request"><see cref="ChannelRequest"/></param>
    /// <returns><c>bool</c></returns>
    public bool CanHandle(ChannelRequest request) => request is ImageChannelRequest;

    
    /// <summary>
    /// Method to pass data through a channel.
    /// </summary>
    /// <param name="request"><see cref="ChannelRequest"/></param>
    /// <returns><see cref="ChannelResponse"/></returns>
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