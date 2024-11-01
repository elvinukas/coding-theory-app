namespace app.Algorithms;

public interface ISimilarity<T>
{
    public static abstract double CalculateSimilarity(T entity, T secondEntity);
}