namespace tests;
using app.Math;
using app.Algorithms;

// basically,
// 

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
    public void RetrieveParityCheckMatrix_CheckIfCorrectMatrixIsRetrieved2()
    {
        int[,] generatorMatrixElements = new int[,]
        {
            {1, 0, 0, 1},
            {1, 1, 0, 0},
            {0, 0, 1, 1}
        };
        Matrix generatorMatrix = new Matrix(generatorMatrixElements);
        Matrix transposedParityMatrix = StepByStepDecodingAlgorithm.RetrieveParityMatrix(generatorMatrix).Transpose();

        Matrix generatedParityCheckMatrix =
            StepByStepDecodingAlgorithm.RetrieveParityCheckMatrix(generatorMatrix, transposedParityMatrix);

        int[,] expectedParityCheckMatrixElements = new int[,]
        {
            {1, 0, 1, 1}
        };

        Matrix expectedParityCheckMatrix = new Matrix(expectedParityCheckMatrixElements);
        
        Assert.True(generatedParityCheckMatrix == expectedParityCheckMatrix);
        
        // // then a check can happen if G * H^T = 0;
        //
        // Matrix transposedParityCheckMatrix = generatedParityCheckMatrix.Transpose();
        // Matrix result = generatorMatrix * transposedParityCheckMatrix;
        //
        // FieldElement zero = new FieldElement(0, new Field(2));
        // for (int row = 0; row < result.Rows; ++row)
        // {
        //     for (int column = 0; column < result.Columns; ++column)
        //     {
        //         Assert.True(result[row, column] == zero);
        //     }
        // }



    }

    
    
    [Fact]
    public void Decode_CheckIfDecodingIsCorrectWhenThereIsAMistake()
    {
        int k = 4;
        
        Matrix originalMessage = new Matrix(new int[,]
        {
            {1, 1, 0, 0, 1}
        });

        Matrix generatorMatrix = new Matrix(new int[,]
        {
            {1, 0, 0, 0, 1, 1, 0},
            {0, 1, 0, 0, 1, 0, 1},
            {0, 0, 1, 0, 1, 1, 1},
            {0, 0, 0, 1, 0, 1, 1}
        });

        LinearEncodingAlgorithm linearEncodingAlgorithm =
            new LinearEncodingAlgorithm(originalMessage, generatorMatrix, k, generatorMatrix.Columns);
        Matrix encodedMessage = linearEncodingAlgorithm.EncodedMessage;
        
        // simulating without a channel, manually inputting a mistake
        Matrix errorVector = new Matrix(new int[,]
        {
            {0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        });

        Matrix receivedMessage = encodedMessage + errorVector;
        
        // decoding process
        StepByStepDecodingAlgorithm algorithm =
            new StepByStepDecodingAlgorithm(generatorMatrix, linearEncodingAlgorithm.OriginalMessageLength);
        Matrix actuallyDecodedMessage = algorithm.DecodeMessage(receivedMessage);
        
        // 0 0 0 0 0 1 0 1 1 1 0 0 1 0 0 - without trimming
        // with trimming it is
        // 1 1 0 0 1 0 0, then after calculations that account padding
        // 1 1 0 0 1
        // ITS correct, the mes
        
        Assert.True(originalMessage == actuallyDecodedMessage);
        
    }


    [Fact]
    public void Decode_CheckIfDecodingIsCorrectWhenThereIsAMistake2()
    {
        Field field = new Field(2);
        int[,] elements1 = { { 1, 1, 0, 0, 1, 0, 1, 0 }};
        int[,] elements2 =
        {
            {1, 0, 0, 0, 1, 1, 0},
            {0, 1, 0, 0, 1, 0, 1},
            {0, 0, 1, 0, 1, 1, 1},
            {0, 0, 0, 1, 0, 1, 1}
        
        };
        
        Matrix originalMessage = new Matrix(elements1, field.q);
        Matrix generatorMatrix = new Matrix(elements2, field.q);
        int dimension = 4;
        int n = 7;

        LinearEncodingAlgorithm algorithm = new LinearEncodingAlgorithm(originalMessage, generatorMatrix,
            dimension, n);
        // n assigning value here does not matter, it can be zero
        // since a generator matrix is provided it does not matter

        Matrix errorVector = new Matrix(new int[,]
        {
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0}
        });

        Matrix encodedMessage = algorithm.EncodedMessage + errorVector;


        StepByStepDecodingAlgorithm decodingAlgorithm =
            new StepByStepDecodingAlgorithm(generatorMatrix, algorithm.OriginalMessageLength);
        Matrix decodedMessage = decodingAlgorithm.DecodeMessage(encodedMessage);

        Assert.True(originalMessage == decodedMessage);

    }




    [Fact]
    public void Decode_TryDecoding()
    {
        Matrix originalMessage = new Matrix(new int[,]
        {
            {1, 1, 0, 0, 1, 1, 0, 0, 0, 1, 1, 1}
        });

        Matrix generatorMatrix = new Matrix(new int[,]
        {

            {1, 0, 0, 0, 1, 1, 0},
            {0, 1, 0, 0, 1, 0, 1},
            {0, 0, 1, 0, 1, 1, 1},
            {0, 0, 0, 1, 0, 1, 1}
        });
        

        LinearEncodingAlgorithm algorithm = new LinearEncodingAlgorithm(originalMessage, generatorMatrix, dimension: 4, n: 7);
        Matrix errorVector = Channel.GetSpecifiedNumOfErrorVector(algorithm.EncodedMessage, 1);
        Matrix sentMessage = algorithm.EncodedMessage + errorVector;

        StepByStepDecodingAlgorithm decodingAlgorithm =
            new StepByStepDecodingAlgorithm(generatorMatrix, algorithm.OriginalMessageLength);
        Matrix decodedMessage = decodingAlgorithm.DecodeMessage(sentMessage);
        
        Assert.True(originalMessage == decodedMessage);





    }


    // this is the problem, how to generate a generator matrix so that its minimal code length is the maximum possible
    // according to k and n
    [Fact]
    public void GetMinimalCodeLength_CheckIfCalculationsAreCorrect1()
    {
        Matrix generatorMatrix = new Matrix(new int[,]
        {
            {1, 0, 0, 0, 1, 1},
            {0, 1, 0, 0, 1, 1},
            {0, 0, 1, 0, 0, 0},
            {0, 0, 0, 1, 1, 0}
        });

        int d = StepByStepDecodingAlgorithm.GetMinimalCodeLength(generatorMatrix);
        
        
        Assert.True(d == 1);


    }
    
    // not complete
    [Fact]
    public void GetMinimalCodeLength_CheckIfCalculationsAreCorrect2()
    {
        Matrix generatorMatrix = new Matrix(new int[,]
        {
            {1, 0, 0, 0, 0, 1, 0},
            {0, 1, 0, 0, 0, 1, 0},
            {0, 0, 1, 0, 0, 0, 1},
            {0, 0, 0, 1, 0, 0, 1},
            {0, 0, 0, 0, 1, 1, 0}
        });
        
        
        

        int d = StepByStepDecodingAlgorithm.GetMinimalCodeLength(generatorMatrix);
        
        Assert.True(d == 2);


    }
    
    [Fact]
    public void GetMinimalCodeLength_CheckIfCalculationsAreCorrect3()
    {
        Matrix generatorMatrix = new Matrix(new int[,]
        {
            {1, 0, 0, 0, 1, 1, 0},
            {0, 1, 0, 0, 1, 0, 1},
            {0, 0, 1, 0, 1, 1, 1},
            {0, 0, 0, 1, 0, 1, 1}
        });
        

        int d = StepByStepDecodingAlgorithm.GetMinimalCodeLength(generatorMatrix);
        
        Assert.True(d == 3);


    }
    
    
    // at its current form as presented in the unit test the algorithm can correct 1 mistake per each part
    // if n were bigger, more mistakes could be made and still be corrected
    // [Fact]
    // public void Decode_CheckIfDecodingIsCorrectWhenThereAreMistakes()
    // {
    //     int k = 3;
    //     
    //     Matrix originalMessage = new Matrix(new int[,]
    //     {
    //         {1, 1, 0, 0, 1}
    //     });
    //
    //     Matrix generatorMatrix = new Matrix(new int[,]
    //     {
    //         {1, 0, 0, 0, 1, 0, 1},
    //         {0, 1, 0, 0, 1, 1, 1},
    //         {0, 0, 1, 1, 1, 0, 1}
    //     });
    //
    //     LinearEncodingAlgorithm linearEncodingAlgorithm =
    //         new LinearEncodingAlgorithm(originalMessage, generatorMatrix, k, generatorMatrix.Columns, numberBitLength: 8);
    //     Matrix encodedMessage = linearEncodingAlgorithm.EncodedMessage;
    //     
    //     // simulating without a channel, manually inputting a mistake
    //     Matrix errorVector = new Matrix(new int[,]
    //     {
    //         {0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
    //
    //     });
    //
    //     Matrix receivedMessage = encodedMessage + errorVector;
    //     
    //     // decoding process
    //
    //     Matrix actuallyDecodedMessage = StepByStepDecodingAlgorithm.Decode(generatorMatrix, receivedMessage, numberBitLength: 8);
    //     
    //     // 0 0 0 0 0 1 0 1 1 1 0 0 1 0 0 - without trimming
    //     // with trimming it is
    //     // 1 1 0 0 1 0 0, then after calculations that account padding
    //     // 1 1 0 0 1
    //     // ITS correct, the mes
    //     
    //     Assert.True(originalMessage == actuallyDecodedMessage);
    //     
    // }
    
    
}