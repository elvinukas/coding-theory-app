using app.Controllers;
using app.Exceptions;
using app.Math;
using app.Models;
using app.Models.Matrix;

namespace app.Services;

/// <summary>
/// This class is a service that generates matrices.
/// </summary>
public class MatrixGenService : IGenerator
{
    private readonly INumGen _numGen;
    
    /// <summary>
    /// Constructor for the class.
    /// </summary>
    /// <param name="numGen"><see cref="INumGen"/>any number generator can be inputted</param>
    public MatrixGenService(INumGen numGen)
    {
        _numGen = numGen;
    }
    
    /// <summary>
    /// Method to generate a matrix.
    /// </summary>
    /// <param name="request"><see cref="MatrixRequest"/></param>
    /// <returns><see cref="MatrixResponse"/></returns>
    /// <exception cref="GeneratorException">Throws if unable to generate random matrix.</exception>
    public MatrixResponse GenerateMatrix(MatrixRequest request)
    {
        try
        {
            GeneratorMatrixGenerator generator = new GeneratorMatrixGenerator(_numGen);
            int rows = request.rows;
            int cols = request.cols;

            Matrix randomGenMatrix = generator.GenerateMatrix(rows, cols);
            List<List<int>> rgmList = MatrixConverter.ConvertTo2DList(randomGenMatrix);

            return new MatrixResponse
            {
                Matrix = rgmList
            };
        }
        catch (Exception ex)
        {
            throw new GeneratorException("Unable to generate random matrix." + ex.Message);
        }
    }
    
    
}