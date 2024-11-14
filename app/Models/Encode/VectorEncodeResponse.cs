namespace app.Models.Encode;

public class VectorEncodeResponse : EncodeResponse
{
    public List<List<int>> EncodedMessage { get; set; }
}