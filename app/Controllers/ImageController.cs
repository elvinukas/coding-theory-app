using Microsoft.AspNetCore.Mvc;

namespace app.Controllers;

[ApiController]
[Route("api/[controller]")]

public class ImageController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;   
    
    public ImageController(IWebHostEnvironment environment)
    {
        _environment = environment;
    }
        
    // getting an image from the temporary files
    [HttpGet("{fileName}")]
    public IActionResult GetImage(string fileName)
    {
        string tempDirectory = Path.Combine(_environment.ContentRootPath, "temp");
        string imageFilePath = Path.Combine(tempDirectory, fileName);

        if (!System.IO.File.Exists(imageFilePath))
        {
            return NotFound();
        }

        FileStream image = System.IO.File.OpenRead(imageFilePath);
        return File(image, "image/bmp");

    }
    
    
}