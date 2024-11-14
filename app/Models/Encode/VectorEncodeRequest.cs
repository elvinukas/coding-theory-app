namespace app.Models.Encode;

public class VectorEncodeRequest : EncodeRequest
{
    public List<List<int>> MessageMatrix { get; set; }
    public List<List<int>> GeneratorMatrix { get; set; }
    public int Dimension { get; set; }
    public int N { get; set; }   
}