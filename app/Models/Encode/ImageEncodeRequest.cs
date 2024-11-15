namespace app.Models.Encode;

public class ImageEncodeRequest : EncodeRequest
{
    public byte[] ImageData { get; set; }
}