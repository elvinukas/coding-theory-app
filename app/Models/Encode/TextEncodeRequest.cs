namespace app.Models.Encode;

public class TextEncodeRequest : EncodeRequest
{
    public string Text { get; set; }
    public List<List<int>> GeneratorMatrix { get; set; }
}