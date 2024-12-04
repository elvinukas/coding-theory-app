using System.Text.Json;
using app.Models.Decode;
using app.Services;
using Microsoft.AspNetCore.Mvc;

namespace app.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DecodingController : ControllerBase
{
    private readonly DecodingServiceFactory _decodingServiceFactory;

    public DecodingController(DecodingServiceFactory decodingServiceFactory)
    {
        _decodingServiceFactory = decodingServiceFactory;
    }

    [HttpPost]
    public IActionResult Decode([FromBody] JsonElement requestJson)
    {
        try
        {
            string type = requestJson.GetProperty("Type").GetString();
            DecodeRequest request;

            switch (type.ToLower())
            {
                case "vector":
                    request = JsonSerializer.Deserialize<VectorDecodeRequest>(requestJson.ToString());
                    break;
                case "text":
                    request = JsonSerializer.Deserialize<TextDecodeRequest>(requestJson.ToString());
                    break;
                default:
                    return BadRequest("Decoding type not supported.");
            }

            var service = _decodingServiceFactory.GetService(request);
            var response = service.Decode(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = "Decoding the message failed: " + ex.Message });
        }
    }
    
    
    
}