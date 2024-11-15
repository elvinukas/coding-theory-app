using app.Controllers;
using app.Exceptions;
using app.Math;
using app.Models;
using app.Models.Matrix;

namespace app.Services;

public class MatrixGenService : IGenerator
{
    private readonly INumGen _numGen;
    
    public MatrixGenService(INumGen numGen)
    {
        _numGen = numGen;
    }
    
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