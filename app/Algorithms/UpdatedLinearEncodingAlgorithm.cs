using System.Runtime.CompilerServices;

namespace app.Algorithms;
using app.Math;


public class UpdatedLinearEncodingAlgorithm : IEncoding
{
    public static Matrix Encode(Matrix originalMessage, Matrix gMatrix)
    {
        if (originalMessage.Rows != 1)
        {
            throw new ArgumentException("The original message must be sent as a vector (matrix with one row).");
        }

        int k = gMatrix.Rows;
        int n = gMatrix.Columns;
        int ogMessageLength = originalMessage.Columns;

        if (gMatrix == null)
        {
            RandomNumberGenerator randomNumberGenerator = new RandomNumberGenerator();
            GeneratorMatrixGenerator matrixGenerator = new GeneratorMatrixGenerator(randomNumberGenerator);
        }

        return originalMessage;

    }

    public static void EncodeFile(string filePath, string encodedFilePath)
    {
        
    }
    
}