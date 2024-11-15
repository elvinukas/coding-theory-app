using System.Text.Json;
using app.Models.Encode;
using app.Services;
using Microsoft.AspNetCore.Mvc;
namespace app.Controllers;



[ApiController]
[Route("api/[controller]")]
public class EncodingController : ControllerBase
{
    private readonly EncodingServiceFactory _encodingServiceFactory;
    
    public EncodingController(EncodingServiceFactory encodingServiceFactory)
    {
        _encodingServiceFactory = encodingServiceFactory;
    }
    
    [HttpPost]
    public IActionResult Encode([FromBody] JsonElement requestJson)
    {
        try
        {
            string type = requestJson.GetProperty("Type").GetString();
            EncodeRequest request;
            
            switch (type.ToLower())
            {
                case "vector":
                    request = JsonSerializer.Deserialize<VectorEncodeRequest>(requestJson.ToString());
                    break;
                // ... and then text and image when they are implemented
                default:
                    return BadRequest("Encoding type not supported.");
            }

            var service = _encodingServiceFactory.GetService(request);
            var response = service.Encode(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = "Encoding failed: " + ex.Message });
        }
        
    }
    

    
    
}