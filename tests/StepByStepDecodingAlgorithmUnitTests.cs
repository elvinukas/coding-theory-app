namespace tests;
using app.Math;
using app.Algorithms;

public class StepByStepDecodingAlgorithmUnitTests
{

    [Fact]
    public void RetrieveParityMatrix_CheckIfCorrectParityMatrixIsRetrieved()
    {
        int[,] generatorMatrixElements = new int[,]
        {
            { 1, 0, 0, 0, 1, 1, 1 },
            {0, 1, 0, 0, 1, 0, 0 },
            {0, 0, 1, 1, 1, 1, 0 }
        };

        Matrix generatorMatrix = new Matrix(generatorMatrixElements);
        
        int[,] expectedParityMatrixElements = new int[,]
        {
            {0, 1, 1, 1},
            {0, 1, 0, 0},
            {1, 1, 1, 0}
        };
        Matrix expectedParityMatrix = new Matrix(expectedParityMatrixElements);
        
        Assert.True(StepByStepDecodingAlgorithm.RetrieveParityMatrix(generatorMatrix) == expectedParityMatrix);


    }

    [Fact]
    public void RetrieveParityCheckMatrix_CheckIfCorrectMatrixIsRetrieved()
    {
        int[,] generatorMatrixElements = new int[,]
        {
            { 1, 0, 0, 0, 1 },
            { 0, 1, 0, 0, 1 },
            { 0, 0, 1, 1, 1 }
        };
        Matrix generatorMatrix = new Matrix(generatorMatrixElements);
        Matrix transposedParityMatrix = StepByStepDecodingAlgorithm.RetrieveParityMatrix(generatorMatrix).Transpose();

        Matrix generatedParityCheckMatrix =
            StepByStepDecodingAlgorithm.RetrieveParityCheckMatrix(generatorMatrix, transposedParityMatrix);

        int[,] expectedParityCheckMatrixElements = new int[,]
        {
            {0, 0, 1, 1, 0},
            {1, 1, 1, 0, 1}
        };

        Matrix expectedParityCheckMatrix = new Matrix(expectedParityCheckMatrixElements);
        
        Assert.True(generatedParityCheckMatrix == expectedParityCheckMatrix);
        
        // then a check can happen if G * H^T = 0;

        Matrix transposedParityCheckMatrix = generatedParityCheckMatrix.Transpose();
        Matrix result = generatorMatrix * transposedParityCheckMatrix;

        FieldElement zero = new FieldElement(0, new Field(2));
        for (int row = 0; row < result.Rows; ++row)
        {
            for (int column = 0; column < result.Columns; ++column)
            {
                Assert.True(result[row, column] == zero);
            }
        }



    }
    
    
}