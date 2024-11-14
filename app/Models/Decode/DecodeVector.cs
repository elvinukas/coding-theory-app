namespace app.Models;

public class DecodeVectorRequest : IDecodeRequest
{
    public List<List<int>> matrix { get; set; }
    public int originalLength { get; set; }
}

public class DecodeVectorResponse : IDecodeResponse
{
    public List<List<int>> matrix { get; set; }
    public string message { get; set; }
}