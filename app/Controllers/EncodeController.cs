using Microsoft.AspNetCore.Mvc;

namespace app.Controllers;
using app.Models;
using app.Math;
using app.Algorithms;


[ApiController]
[Route("api/[controller]")]
public class EncodingController : ControllerBase
{
    [HttpPost("encodevector")]
    public IActionResult EncodeVector([FromBody] EncodeRequest request)
    {
        Matrix messageMatrix = new Matrix(request.MessageMatrix, 2);
        Matrix gMatrix = new Matrix(request.GeneratorMatrix, 2);
        
        try
        {
           
            var encoder = new LinearEncodingAlgorithm(
                messageMatrix,
                gMatrix,
                request.Dimension,
                request.N
            );


            Matrix encodedMessage = encoder.EncodedMessage;
            
            return Ok(new EncodeResponse
            {
                EncodedMessage = encodedMessage,
                Message = "Message encoded successfully."
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
        
    }
    
}