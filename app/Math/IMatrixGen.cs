namespace app.Math;

/// <summary>
/// Interface for any matrix generator.
/// </summary>
public interface IMatrixGen
{
    /// <summary>
    /// Defined method to generate a matrix.
    /// </summary>
    /// <param name="rows">Matrix rows.</param>
    /// <param name="cols">Matrix columns.</param>
    /// <returns><c>Matrix</c></returns>
    Matrix GenerateMatrix(int rows, int cols);
}