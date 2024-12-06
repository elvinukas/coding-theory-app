namespace app.Models;

public class ImageChannelRequest : ChannelRequest
{
    public string FileName { get; set; }
    public double ErrorPercentage { get; set; }
    public List<List<int>>? GeneratorMatrix { get; set; }
}