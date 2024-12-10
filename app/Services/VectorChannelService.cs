using app.Controllers;
using app.Exceptions;
using app.Math;
using app.Models;

namespace app.Services;

/// <summary>
/// Class implementing a channel service for vectors. Can be used for text too, if it is converted to vectors.
/// </summary>
public class VectorChannelService : IChannelService
{
    /// <summary>
    /// Checking whether the service can handle the request.
    /// </summary>
    /// <param name="request"><see cref="ChannelRequest"/></param>
    /// <returns><c>bool</c></returns>
    public bool CanHandle(ChannelRequest request) => request is VectorChannelRequest;
    
    /// <summary>
    /// Method to pass data through a channel.
    /// </summary>
    /// <param name="request"><see cref="ChannelRequest"/></param>
    /// <returns><see cref="ChannelResponse"/></returns>
    public ChannelResponse PassThrough(ChannelRequest request)
    {
        try
        {
            var channelRequest = (VectorChannelRequest)request;
            int[,] matrixArray = MatrixConverter.ConvertToIntArray(channelRequest.Matrix);
            Matrix matrix = new Matrix(matrixArray);
            double errorProb = channelRequest.errorPercentage;

            Channel channel = new Channel(matrix, errorProb);
            Matrix receivedMessage = channel.ReceivedMessage;
            List<List<int>> receivedMessageList = MatrixConverter.ConvertTo2DList(receivedMessage);

            return new VectorChannelResponse
            {
                Matrix = receivedMessageList,
                Message = "Message successfully passed through the channel.",
                Type = "vector"
            };
        }
        catch (Exception e)
        {
            throw new ChannelException("Message could not be passed through the channel.");
        }
    }
    
    
}