namespace app.Algorithms;

/// <summary>
/// Interface to calculate simularity between objects.
/// </summary>
/// <typeparam name="T">Object which is to be compared by similarity.</typeparam>
public interface ISimilarity<T>
{
    /// <summary>
    /// Abstract method to calculate similarity.
    /// </summary>
    /// <param name="entity">Object which is to be compared by similarity.</param>
    /// <param name="secondEntity">Object which is to be compared by similarity.</param>
    /// <returns><c>double</c></returns>
    public static abstract double CalculateSimilarity(T entity, T secondEntity);
}