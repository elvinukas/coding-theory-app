using app.Math;
using app.Models;
using Microsoft.AspNetCore.Mvc;

namespace app.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChannelController : ControllerBase
{
    [HttpPost]
    public IActionResult PassThroughChannel([FromBody] ChannelRequest request)
    {
        try
        {
            int[,] matrixArray = MatrixConverter.ConvertToIntArray(request.Matrix);
            Matrix matrix = new Matrix(matrixArray);
            double errorProb = request.errorPercentage;

            Channel channel = new Channel(matrix, errorProb);
            Matrix receivedMessage = channel.ReceivedMessage;
            List<List<int>> receivedMessageList = MatrixConverter.ConvertTo2DList(receivedMessage);

            return Ok(new ChannelResponse
            {
                Matrix = receivedMessageList,
                Message = "Message successfully passed through the channel."
            });
        }
        catch (Exception e)
        {
            return BadRequest("Message could not be passed through the channel.");
        }
        

    }
    
}