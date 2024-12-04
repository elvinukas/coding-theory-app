namespace app.Models.Encode;

public class TextEncodeResponse : EncodeResponse
{
    public List<List<int>> EncodedMessage { get; set; }
    public List<List<int>> OriginalMessageBinary { get; set; }
    public int OriginalMessageBinaryLength { get; set; }
}