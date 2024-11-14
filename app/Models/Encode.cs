using app.Math;

namespace app.Models;

public class EncodeRequest
{
    public List<List<int>> MessageMatrix { get; set; }
    public List<List<int>> GeneratorMatrix { get; set; }
    public int Dimension { get; set; }
    public int N { get; set; }
}

public class EncodeResponse
{
    public List<List<int>> EncodedMessage { get; set; }
    public string Message { get; set; }
}