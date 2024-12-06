using app.Algorithms;
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
        string extensionType = GetExtensionType(fileName);
        string extension = Path.GetExtension(fileName).ToLowerInvariant();
        FileStream image;

        if (!System.IO.File.Exists(imageFilePath))
        {
            return NotFound();
        }
        
        if (extension == ".bin" && !fileName.EndsWith("_encoded.bin"))
        {
            string savePath = Path.Combine(tempDirectory, fileName.Split(".")[0] + "_raw.bmp");
            ImageConverter.SaveImage(imageFilePath, savePath);
            image = System.IO.File.OpenRead(savePath);
            return File(image, "image/bmp");
        }

        image = System.IO.File.OpenRead(imageFilePath);
        return File(image, extensionType);

    }

    public string GetExtensionType(string fileName)
    {
        string extension = Path.GetExtension(fileName).ToLowerInvariant();
        return extension switch
        {
            ".bmp" => "image/bmp",
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            _ => "application/octet-stream",
        };
    }
    
    
}