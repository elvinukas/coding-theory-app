using Microsoft.AspNetCore.Mvc;

namespace app.Controllers;
using app.Models;
using app.Math;
using app.Algorithms;
using Newtonsoft.Json.Linq;


[ApiController]
[Route("api/[controller]")]
public class EncodingController : ControllerBase
{
    [HttpPost("encodevector")]
    public IActionResult EncodeVector([FromBody] EncodeRequest request)
    {
        int[,] messageMatrixArray = ConvertToIntArray(request.MessageMatrix);
        int[,] generatorMatrixArray = ConvertToIntArray(request.GeneratorMatrix);
        
        Matrix messageMatrix = new Matrix(messageMatrixArray, 2);
        Matrix gMatrix = new Matrix(generatorMatrixArray, 2);
        
        try
        {
            Matrix encodedMessage = UpdatedLinearEncodingAlgorithm.Encode(messageMatrix, gMatrix);
            List<List<int>> encodedMessageList = ConvertTo2DList(encodedMessage);
            
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

    private static int[,] ConvertToIntArray(List<List<int>> matrix)
    {
        int rows = matrix.Count;
        int cols = matrix[0].Count;
        int[,] matrixArray = new int[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                matrixArray[i, j] = matrix[i][j];
            }
        }

        return matrixArray;
    }

    private static List<List<int>> ConvertTo2DList(Matrix matrix)
    {
        List<List<int>> list = new List<List<int>>();

        for (int i = 0; i < matrix.Rows; ++i)
        {
            list.Add(new List<int>());
            for (int j = 0; j < matrix.Columns; ++j)
            {
                list[i].Add(matrix[i, j].Value);
            }
        }

        return list;


    }
    
}