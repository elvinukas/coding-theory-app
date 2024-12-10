using System.Text.Json;
using app.Models.Decode;
using app.Services;
using Microsoft.AspNetCore.Mvc;

namespace app.Controllers;


/// <summary>
/// Controller class which is used to decode matrices through an API.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class DecodingController : ControllerBase
{
    private readonly DecodingServiceFactory _decodingServiceFactory;

    /// <summary>
    /// Constructor for the decoding controller. The controller uses the factory design pattern to determine which
    /// service to use for decoding.
    /// </summary>
    /// <param name="decodingServiceFactory"><see cref="DecodingServiceFactory"/></param>
    public DecodingController(DecodingServiceFactory decodingServiceFactory)
    {
        _decodingServiceFactory = decodingServiceFactory;
    }

    /// <summary>
    /// HTTP-POST request method to decode matrices.
    /// </summary>
    /// <param name="requestJson"><c>DecodeRequest</c> or its children classes</param>
    /// <returns></returns>
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
                case "image":
                    request = JsonSerializer.Deserialize<ImageDecodeRequest>(requestJson.ToString());
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