namespace tests;
using app.Math;
using app.Algorithms;

public class StepByStepDecodingAlgorithmUnitTests
{

    [Fact]
    public void GetOriginalEncodedMessage_CheckIfEncodedMessageRetrievalIsCorrect()
    {
        Matrix originalMessage = new Matrix(new int[,] { {1, 1, 0, 0, 1, 0 }}); // without the extra 0 is a hard case
        // for now test only with divisible amount message

        Matrix generatorMatrix = new Matrix(new int[,]
        {
            {1, 0, 0, 0, 1},
            {0, 1, 0, 0, 1},
            {0, 0, 1, 1, 1}
        });

        LinearEncodingAlgorithm linearEncodingAlgorithm =
            new LinearEncodingAlgorithm(originalMessage, generatorMatrix, 3);

        // this is the message that will be checked if it is error-fixed
        Matrix encodedMessage = linearEncodingAlgorithm.EncodedMessage;
        
        // for now just checking the getting the original encoded message
        // so no channel will be used
        // the real error vector added will be for a message part,
        // however for testing purposes, the added error vector will be the length of the full messsage

        Matrix errorVector = new Matrix(new int[,]
        {
            {0, 0, 0, 1, 0, 0, 0, 0, 0, 0}
        });

        Matrix encodedMessageWithErrors = encodedMessage + errorVector;
        
        Assert.True(
            StepByStepDecodingAlgorithm
                .GetOriginalEncodedMessage(generatorMatrix, encodedMessageWithErrors) == encodedMessage);
        
        
    }
    
    
    
}