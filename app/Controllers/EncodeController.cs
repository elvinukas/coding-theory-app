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
        int[,] messageMatrixArray = MatrixConverter.ConvertToIntArray(request.MessageMatrix);
        int[,] generatorMatrixArray = MatrixConverter.ConvertToIntArray(request.GeneratorMatrix);
        
        Matrix messageMatrix = new Matrix(messageMatrixArray, 2);
        Matrix gMatrix = new Matrix(generatorMatrixArray, 2);
        
        try
        {
            Matrix encodedMessage = UpdatedLinearEncodingAlgorithm.Encode(messageMatrix, gMatrix);
            List<List<int>> encodedMessageList = MatrixConverter.ConvertTo2DList(encodedMessage);
            
            return Ok(new EncodeResponse
            {
                EncodedMessage = encodedMessageList,
                Message = "Message encoded successfully."
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = "Vector encoding failed: " + ex.Message });
        }
        

    }
    

    
    
}