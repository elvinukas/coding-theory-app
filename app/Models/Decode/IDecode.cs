using Microsoft.AspNetCore.Mvc;

namespace app.Models;

public interface IDecodeRequest
{
    public List<List<int>> matrix { get; set; }
    public int originalLength { get; set; }
}

public interface IDecodeResponse
{
    public List<List<int>> matrix { get; set; }
    public string message { get; set; }
}