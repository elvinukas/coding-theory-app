namespace app.Models;

public class VectorChannelRequest : ChannelRequest
{
    public List<List<int>> Matrix { get; set; }
    public double errorPercentage { get; set; }
}