using System.Text.Json;
using app.Math;
using app.Models;
using app.Services;
using Microsoft.AspNetCore.Mvc;

namespace app.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChannelController : ControllerBase
{
    private readonly ChannelServiceFactory _channelServiceFactory;
    
    public ChannelController(ChannelServiceFactory channelServiceFactory)
    {
        _channelServiceFactory = channelServiceFactory;
    }
    
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