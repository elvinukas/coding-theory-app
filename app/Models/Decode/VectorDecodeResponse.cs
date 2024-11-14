namespace app.Models.Decode;

public class VectorDecodeResponse : DecodeResponse
{
    public List<List<int>> DecodedMessage { get; set; }
}