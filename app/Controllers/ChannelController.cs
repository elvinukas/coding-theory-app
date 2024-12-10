using System.Text.Json;
using app.Math;
using app.Models;
using app.Services;
using Microsoft.AspNetCore.Mvc;

namespace app.Controllers;


/// <summary>
/// Controller class which is used to call the <c>Channel</c> class through an API.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ChannelController : ControllerBase
{
    private readonly ChannelServiceFactory _channelServiceFactory;
    
    /// <summary>
    /// Constructor for the controller. Controller uses the factory design pattern to determine which service is used.
    /// </summary>
    /// <param name="channelServiceFactory">Takes in a channel service factory.</param>
    public ChannelController(ChannelServiceFactory channelServiceFactory)
    {
        _channelServiceFactory = channelServiceFactory;
    }
    
    /// <summary>
    /// HTTP-POST request method that allows passing matrix through a channel.
    /// Responds with <see cref="ChannelResponse"/> or any of its child classes.
    /// </summary>
    /// <param name="requestJson">
    /// Takes in <see cref="ChannelRequest"/> or any of its child classes.
    /// </param>
    /// <returns><c>IActionResult</c></returns>
    /// <seealso cref="VectorChannelRequest"/>
    /// <seealso cref="ImageChannelRequest"/>
    /// <seealso cref="VectorChannelResponse"/>
    /// <seealso cref="ImageChannelResponse"/>
    [HttpPost]
    public IActionResult PassThroughChannel([FromBody] JsonElement requestJson)
    {
        try
        {
            string type = requestJson.GetProperty("Type").GetString();
            ChannelRequest request;

            switch (type.ToLower())
            {
                case "vector":
                    request = JsonSerializer.Deserialize<VectorChannelRequest>(requestJson.ToString());
                    break;
                case "image":
                    request = JsonSerializer.Deserialize<ImageChannelRequest>(requestJson.ToString());
                    break;
                default:
                    return BadRequest("Channel type not supported.");
            }

            var service = _channelServiceFactory.GetService(request);
            var response = service.PassThrough(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = "Passing through the channel failed: " + ex.Message });
        }
        
    }
    
}