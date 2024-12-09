namespace app.Models.Graph;

public struct GraphResponse
{
    public string Message { get; set; }
    public string FileName { get; set; }
    public double? CrossErrorPercentage { get; set; }
    
    
    // [og_error_percentage, final_error_percentage]
    //public List<List<double>> Values { get; set; }
}