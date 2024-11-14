using app.Controllers;
using app.Exceptions;
using app.Math;
using app.Models;

namespace app.Services;

public class VectorChannelService : IChannelService
{
    public bool CanHandle(ChannelRequest request) => request is VectorChannelRequest;
    
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