namespace app.Models.Encode;

public class TextEncodeResponse : EncodeResponse
{
    public List<List<int>> EncodedMessage { get; set; }
}