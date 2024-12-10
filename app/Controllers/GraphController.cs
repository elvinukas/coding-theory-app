using System.Text.Json;
using app.Models.Graph;
using app.Services;
using Microsoft.AspNetCore.Mvc;

namespace app.Controllers;


/// <summary>
/// Controller class which is used to draw graphs for encoding/decoding testing through an API.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class GraphController : ControllerBase
{
    private readonly IGraphService _service;

    /// <summary>
    /// Contructor method which accepts the graph service which will paint the graph.
    /// </summary>
    /// <param name="service"><see cref="IGraphService"/></param>
    public GraphController(IGraphService service)
    {
        _service = service;
    }

    /// <summary>
    /// Method to paint a graph based on the requested data.
    /// </summary>
    /// <param name="request"><see cref="GraphRequest"/></param>
    /// <returns><see cref="IActionResult"/></returns>
    public IActionResult Paint([FromBody] GraphRequest request)
    {
        try
        {
            GraphResponse response = _service.Paint(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = "Creating a graph failed. Error: " + ex.Message });
        }
        
    }
    
}