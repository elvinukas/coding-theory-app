using app.Models.Graph;

namespace app.Services;

public interface IGraphService
{
    GraphResponse Paint(GraphRequest request);
}