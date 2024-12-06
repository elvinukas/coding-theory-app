namespace app.Models.Decode;

public class ImageDecodeRequest : DecodeRequest
{
    public string FileName { get; set; }
    public List<List<int>> GeneratorMatrix { get; set; }
}