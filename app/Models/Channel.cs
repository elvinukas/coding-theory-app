namespace app.Models;

public class ChannelRequest
{
    public List<List<int>> Matrix { get; set; }
    public double errorPercentage { get; set; }
}

public class ChannelResponse
{
    public List<List<int>> Matrix { get; set; }
    public string Message { get; set; }
}