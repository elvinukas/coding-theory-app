using app.Exceptions;
using app.Math;
using app.Models.Matrix;
using app.Services;
using Microsoft.AspNetCore.Mvc;

namespace app.Controllers;


/// <summary>
/// Controller class to retrieve random generator matrices from the backend using an API.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class MatrixController : ControllerBase
{

    private readonly IGenerator _generator;
    
    /// <summary>
    /// Constructor which can be provided an interface for generating matrices.
    /// </summary>
    /// <param name="generator"><see cref="IGenerator"/></param>
    public MatrixController(IGenerator generator)
    {
        _generator = generator;
    }

    /// <summary>
    /// HTTP-POST method to return a randomly generated matrix.
    /// </summary>
    /// <param name="request"><see cref="MatrixRequest"/></param>
    /// <returns></returns>
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