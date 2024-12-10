using System.Text.Json;
using app.Algorithms;
using app.Math;
using app.Models.Matrix;
using Microsoft.AspNetCore.Mvc;

namespace app.Controllers;


/// <summary>
/// Controller to convert a matrix into a binary array or vice versa.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BinaryController : ControllerBase
{

    /// <summary>
    /// HTTP-POST request to convert a matrix to a string.
    /// </summary>
    /// <param name="request"></param>
    /// <returns><see cref="IActionResult"/></returns>
    [HttpPost("toString")]
    public IActionResult ConvertToString([FromBody] BinaryConverterRequest request)
    {
        try
        {
            int[,] messageMatrixArray = MatrixConverter.ConvertToIntArray(request.Message);
            Matrix messageMatrix = new Matrix(messageMatrixArray);

            string message = TextConverter.ConvertToOriginalFormat(messageMatrix);

            BinaryConverterResponse response = new BinaryConverterResponse
            {
                Message = message
            };
            
            return Ok(response);

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        
        
        
    }
}