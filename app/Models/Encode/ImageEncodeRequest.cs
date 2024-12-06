namespace app.Models.Encode;

public class ImageEncodeRequest : EncodeRequest
{
    public IFormFile Image { get; set; }
    public List<List<int>> Matrix { get; set; }
}