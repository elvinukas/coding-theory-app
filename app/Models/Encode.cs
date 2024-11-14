using app.Math;

namespace app.Models;

public class EncodeRequest
{
    public int[,] MessageMatrix { get; set; }
    public int[,] GeneratorMatrix { get; set; }
    public int Dimension { get; set; }
    public int N { get; set; }
}

public class EncodeResponse
{
    public Matrix EncodedMessage { get; set; }
    public string Message { get; set; }
}