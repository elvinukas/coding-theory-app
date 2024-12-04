namespace app.Models.Decode;

public class TextDecodeRequest : DecodeRequest
{
    public List<List<int>> MessageMatrix { get; set; }
    public List<List<int>> GeneratorMatrix { get; set; }
    public int Length { get; set; }
    
}