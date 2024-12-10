using app.Models.Matrix;

namespace app.Services;


/// <summary>
/// Interface for implementing a generator.
/// </summary>
public interface IGenerator
{
    /// <summary>
    /// Method to generate a matrix.
    /// </summary>
    /// <param name="request"><see cref="MatrixRequest"/></param>
    /// <returns><see cref="MatrixResponse"/></returns>
    MatrixResponse GenerateMatrix(MatrixRequest request);
}