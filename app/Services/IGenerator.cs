using app.Models.Matrix;

namespace app.Services;

public interface IGenerator
{
    MatrixResponse GenerateMatrix(MatrixRequest request);
}