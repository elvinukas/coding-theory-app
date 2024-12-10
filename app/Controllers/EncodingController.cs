using System.Text.Json;
using app.Models.Encode;
using app.Services;
using Microsoft.AspNetCore.Mvc;
namespace app.Controllers;


/// <summary>
/// Controller class which is used to encode matrices through an API.
/// </summary>

[ApiController]
[Route("api/[controller]")]
public class EncodingController : ControllerBase
{
    private readonly EncodingServiceFactory _encodingServiceFactory;
    
    /// <summary>
    /// Constructor for the encoding controller. The controller uses the factory design pattern to determine which
    /// service to use for encoding.
    /// </summary>
    /// <param name="encodingServiceFactory"><see cref="EncodingServiceFactory"/></param>
    public EncodingController(EncodingServiceFactory encodingServiceFactory)
    {
        _encodingServiceFactory = encodingServiceFactory;
    }
    
    /// <summary>
    /// HTTP-POST request to encode matrices or files.
    /// <para>Can be sent a json file, or in the case of image encoding -
    /// image files through <c>Request.Form</c>.</para>
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public IActionResult Encode()
    {
        try
        {
            if (Request.HasFormContentType)
            {
                var form = Request.Form;
                var imageFile = form.Files["image"];
                var matrixJson = form["matrix"];
                List<List<int>> matrix = JsonSerializer.Deserialize<List<List<int>>>(matrixJson);
                
                var imageRequest = new ImageEncodeRequest
                {
                    Image = imageFile,
                    Matrix = matrix
                };
                
                var service = _encodingServiceFactory.GetService(imageRequest);
                var response = service.Encode(imageRequest);
                return Ok(response);
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var requestBody = reader.ReadToEndAsync().Result;
                var requestJson = JsonSerializer.Deserialize<JsonElement>(requestBody);

                if (!requestJson.TryGetProperty("Type", out var typeProperty))
                {
                    return BadRequest("Invalid JSON data.");
                }

                string type = typeProperty.GetString()?.ToLower();
                EncodeRequest request;
            
                switch (type.ToLower())
                {
                    case "vector":
                        request = JsonSerializer.Deserialize<VectorEncodeRequest>(requestJson.ToString());
                        break;
                    case "text":
                        request = JsonSerializer.Deserialize<TextEncodeRequest>(requestJson.ToString());
                        break;
                    default:
                        return BadRequest("Encoding type not supported.");
                }

                var service = _encodingServiceFactory.GetService(request);
                var response = service.Encode(request);
                return Ok(response); 
            }
            
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = "Encoding failed: " + ex.Message });
        }
        
    }
    

    
    
}