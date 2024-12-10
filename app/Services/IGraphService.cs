using app.Models.Graph;

namespace app.Services;

/// <summary>
/// Interface for implementing a graph painter.
/// </summary>
public interface IGraphService
{
    /// <summary>
    /// Method to paint a graph.
    /// </summary>
    /// <param name="request"><see cref="GraphRequest"/></param>
    /// <returns><see cref="GraphResponse"/></returns>
    GraphResponse Paint(GraphRequest request);
}