using app.Algorithms;
using Microsoft.AspNetCore.Mvc;

namespace app.Controllers;


/// <summary>
/// Controller class which is used to retrieve images from the backend using an API.
/// </summary>
[ApiController]
[Route("api/[controller]")]

public class ImageController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;   
    
    /// <summary>
    /// Constructor which takes in a web host environment.
    /// </summary>
    /// <param name="environment"><see cref="IWebHostEnvironment"/></param>
    public ImageController(IWebHostEnvironment environment)
    {
        _environment = environment;
    }
        
    // getting an image from the temporary files
    /// <summary>
    /// Method to retrieve an image from the temporary saved files.
    /// </summary>
    /// <param name="fileName">Name of the file being retrieved.</param>
    /// <returns><see cref="IActionResult"/></returns>
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

    /// <summary>
    /// Method to retrieve the extension type from a file name.
    /// </summary>
    /// <param name="fileName">File name.</param>
    /// <returns><c>string</c>, which represents the extension type.</returns>
    /// <example>When file name is "hi.bmp", returns ".bmp".</example>
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