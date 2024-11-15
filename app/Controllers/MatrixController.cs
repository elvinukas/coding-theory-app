using app.Exceptions;
using app.Math;
using app.Models.Matrix;
using app.Services;
using Microsoft.AspNetCore.Mvc;

namespace app.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MatrixController : ControllerBase
{

    private readonly IGenerator _generator;
    
    public MatrixController(IGenerator generator)
    {
        _generator = generator;
    }

    [HttpPost]
    public IActionResult GetRandomGenMatrix([FromBody] MatrixRequest request)
    {
        try
        {
            MatrixResponse response = _generator.GenerateMatrix(request);


            return Ok(response);

        }
        catch (GeneratorException ex)
        {
            return BadRequest("Unable to generate random matrix.");
        }
        
        
    }
    
    
    
    
    
}