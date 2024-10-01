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


    [Fact]
    public void Decode_CheckIfDecodingIsCorrectWhenThereIsAMistake()
    {
        int k = 3;
        
        Matrix originalMessage = new Matrix(new int[,]
        {
            {1, 1, 0, 0, 1}
        });

        Matrix generatorMatrix = new Matrix(new int[,]
        {
            {1, 0, 0, 0, 1},
            {0, 1, 0, 0, 1},
            {0, 0, 1, 1, 1,}
        });

        LinearEncodingAlgorithm linearEncodingAlgorithm =
            new LinearEncodingAlgorithm(originalMessage, generatorMatrix, k, generatorMatrix.Columns);
        Matrix encodedMessage = linearEncodingAlgorithm.EncodedMessage;
        
        // simulating without a channel, manually inputting a mistake
        Matrix errorVector = new Matrix(new int[,]
        {
            {0, 0, 0, 1, 0, 0, 0, 0, 0, 0}
        });

        Matrix receivedMessage = encodedMessage + errorVector;
        
        // decoding process
        Matrix expectedDecodedMessage = new Matrix(new int[,]
        {
            {1, 1, 0, 0, 1, 0}
        });

        Matrix actuallyDecodedMessage = StepByStepDecodingAlgorithm.Decode(generatorMatrix, receivedMessage);
        
        Assert.True(expectedDecodedMessage == actuallyDecodedMessage);
        
    }
    
    
    // at its current form as presented in the unit test the algorithm can correct 1 mistake per each part
    // if n were bigger, more mistakes could be made and still be corrected
    [Fact]
    public void Decode_CheckIfDecodingIsCorrectWhenThereAreMistakes()
    {
        int k = 3;
        
        Matrix originalMessage = new Matrix(new int[,]
        {
            {1, 1, 0, 0, 1}
        });

        Matrix generatorMatrix = new Matrix(new int[,]
        {
            {1, 0, 0, 0, 1},
            {0, 1, 0, 0, 1},
            {0, 0, 1, 1, 1,}
        });

        // n does not matter if generator matrix is provided!
        // it will assign n itself
        LinearEncodingAlgorithm linearEncodingAlgorithm =
            new LinearEncodingAlgorithm(originalMessage, generatorMatrix, k, generatorMatrix.Columns);
        Matrix encodedMessage = linearEncodingAlgorithm.EncodedMessage;
        
        
        Matrix errorVector = new Matrix(new int[,]
        {
            {0, 0, 0, 1, 0, 0, 0, 1, 0, 0}
        });

        Matrix receivedMessage = encodedMessage + errorVector;
        
        // decoding process
        Matrix expectedDecodedMessage = new Matrix(new int[,]
        {
            {1, 1, 0, 0, 1, 0}
        });

        Matrix actuallyDecodedMessage = StepByStepDecodingAlgorithm.Decode(generatorMatrix, receivedMessage);
        
        Assert.True(expectedDecodedMessage == actuallyDecodedMessage);
        
    }
    
    
}