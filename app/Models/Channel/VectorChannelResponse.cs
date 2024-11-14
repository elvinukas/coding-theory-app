namespace app.Models;

public class VectorChannelResponse : ChannelResponse
{
    public List<List<int>> Matrix { get; set; }
}