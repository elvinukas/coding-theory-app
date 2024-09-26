namespace tests;
using app.Algorithms;
using app.Math;

public class LinearEncodingAlgorithmUnitTest
{
    [Fact]
    public void Constructor_CheckIfOriginalMessageIsAVector()
    {
        Field field = new Field(5);
        int[,] elements1 = { { 1, 0, 2, 3, 4 }, { 1, 1, 0, 3, 2 }, { 3, 1, 1, 2, 1 }, { 0, 1, 4, 0, 1 } };
        int[,] elements2 = { { 1, 0, 4, 1, 3 }, { 3, 4, 1, 1, 0}, { 0, 4, 1, 1, 0 }, { 1, 1, 2, 3, 0 } };
        Matrix originalMessage = new Matrix(elements1, field.q);
        Matrix generatorMatrix = new Matrix(elements2, field.q);
        int dimension = 4;

        var exception = Assert.Throws<ArgumentException>(() => new LinearEncodingAlgorithm(originalMessage, generatorMatrix, dimension));
        Assert.Equal("The original message must be sent as a vector (matrix with one row).", exception.Message);

    }

    [Fact]
    public void Constructor_CheckIfDimensionCountIsCorrect()
    {
        Field field = new Field(5);
        int[,] elements1 = { {1, 3, 2, 1, 0, 2, 1, 1}};
        int[,] elements2 = { { 1, 0, 4, 1, 3 }, { 3, 4, 1, 1, 0}, { 0, 4, 1, 1, 0 }, { 1, 1, 2, 3, 0 } };
        Matrix originalMessage = new Matrix(elements1, field.q);
        Matrix generatorMatrix = new Matrix(elements2, field.q);
        int dimension = 0;

        var exception = Assert.Throws<ArgumentException>(() => new LinearEncodingAlgorithm(originalMessage, generatorMatrix, dimension));
        Assert.Equal("The dimension count k cannot be less or equal than 0.", exception.Message);
    }

    [Fact]
    public void Constructor_CheckIfGeneratorMatrixIsCorrect()
    {
        Field field = new Field(2);
        int[,] elements1 = { { 1, 0, 1 }};
        int[,] elements2 = { { 1, 0, 1, 1, 0 }, { 0, 1, 1, 1, 0 }, { 0, 1, 1, 1, 0 }, { 1, 0, 1, 0, 1 } };
        Matrix originalMessage = new Matrix(elements1, field.q);
        Matrix generatorMatrix = new Matrix(elements2, field.q);
        int dimension = 1;

        var exception = Assert.Throws<ArithmeticException>(() => new LinearEncodingAlgorithm(originalMessage, generatorMatrix, dimension));
        Assert.Equal("Matrix multiplication is not possible. The number of First Matrix columns must equal the number of rows in the Second Matrix", exception.Message);
        
    }
    
    // easy test, dimension = 3 (meaning the each divided part is 3 long, so no division will happen), still k / n divides
    [Fact]
    public void Constructor_CheckIfEncodedMessageIsCorrect()
    {
        Field field = new Field(2);
        int[,] elements1 = { { 1, 1, 0 }};
        int[,] elements2 = { { 1, 1, 0, 0 }, { 0, 1, 1, 1 }, { 1, 0, 1, 0 } };
        Matrix originalMessage = new Matrix(elements1, field.q);
        Matrix generatorMatrix = new Matrix(elements2, field.q);
        int dimension = 3;

        LinearEncodingAlgorithm algorithm = new LinearEncodingAlgorithm(originalMessage, generatorMatrix, dimension);
        Assert.Equal("1 0 1 1 \n", algorithm.encodedMessage.ToString());
        
        
    }

    // harder test - k / n divides, dimension = 1. each element from elements1 is seperately being encoded
    [Fact]
    public void Constructor_CheckIfEncodedMessageIsCorrect2()
    {
        Field field = new Field(2);
        int[,] elements1 = { { 1, 1, 0 }};
        //int[,] elements2 = { { 1, 1, 0, 0 }, { 0, 1, 1, 1 }, { 1, 0, 1, 0 } }; // this generator matrix does not work, since each element is now being matrix multiplied
        int[,] elements2 = { { 1, 1, 0, 0 } };
        Matrix originalMessage = new Matrix(elements1, field.q);
        Matrix generatorMatrix = new Matrix(elements2, field.q);
        int dimension = 1;

        LinearEncodingAlgorithm algorithm = new LinearEncodingAlgorithm(originalMessage, generatorMatrix, dimension);
        Assert.Equal("1 1 0 0 1 1 0 0 0 0 0 0 \n", algorithm.encodedMessage.ToString());
        
        
    }
    
    
    // even harder unit test, dimension = 2, k / n has a remainder
    // we will have a longer encoded message
    [Fact]
    public void Constructor_CheckIfEncodedMessageIsCorrect3()
    {
        Field field = new Field(2);
        int[,] elements1 = { { 1, 1, 0 }};
        int[,] elements2 = { { 1, 1, 0, 0 }, { 0, 1, 1, 1 } };
        Matrix originalMessage = new Matrix(elements1, field.q);
        Matrix generatorMatrix = new Matrix(elements2, field.q);
        int dimension = 2;
        
        // the algorithm will encode a message with a bigger length than the originalMessage
        // 1 1 0 0

        LinearEncodingAlgorithm algorithm = new LinearEncodingAlgorithm(originalMessage, generatorMatrix, dimension);
        Assert.Equal("1 0 1 1 0 0 0 0 \n", algorithm.encodedMessage.ToString());
        
        
    }
    
    

    [Fact]
    public void GetCorrectSizeMessageForEncoding_CheckIfDimensionCountIsNotLargerThanMessageLength()
    {
        Field field = new Field(5);
        int[,] elements1 = { {1, 3, 2, 1, 0, 2, 1, 1}};
        int[,] elements2 = { { 1, 0, 4, 1, 3 }, { 3, 4, 1, 1, 0}, { 0, 4, 1, 1, 0 }, { 1, 1, 2, 3, 0 } };
        Matrix originalMessage = new Matrix(elements1, field.q);
        Matrix generatorMatrix = new Matrix(elements2, field.q);
        int dimension = 100;
        
        // GetCorrectSizeMessageForEncoding throws the exception, which gets passed on to the constructor
        var exception = Assert.Throws<ArgumentException>(() => new LinearEncodingAlgorithm(originalMessage, generatorMatrix, dimension));
        Assert.Equal("The dimension count k cannot be larger than the vector length.", exception.Message);
        
    }

    [Fact]
    public void GetCorrectSizeMessageForEncoding_CheckIfLengthCorrectIfDivisible()
    {
        
    }

    [Fact]
    public void GetCorrectSizeMessegeForEncoding_CheckIfLengthCorrectIfNotDivisible()
    {
        
    }
    
    
}