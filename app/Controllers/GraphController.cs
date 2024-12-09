using System.Text.Json;
using app.Models.Graph;
using app.Services;
using Microsoft.AspNetCore.Mvc;

namespace app.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GraphController : ControllerBase
{
    private readonly IGraphService _service;

    public GraphController(IGraphService service)
    {
        _service = service;
    }

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