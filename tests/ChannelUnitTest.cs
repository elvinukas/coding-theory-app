namespace tests;
using app.Math;
using Moq; // third party library used for testing randomness

public class ChannelUnitTest
{
    [Fact]
    public void Constructor_CheckIfProbabilityIsCorrect()
    {
        int[,] elements1 = { { 1, 0, 1, 0, 1 } };
        Matrix testMessageMatrix = new Matrix(elements1);
        double probability = 1.2;
        var exception = Assert.Throws<ArgumentException>(() => new Channel(testMessageMatrix, probability));
        Assert.Equal("The probability of an error inside a channel must be between 0 and 1.", exception.Message);
    }

    [Fact]
    public void Constructor_CheckIfEncodedMessageIsAVector()
    {
        int[,] elements1 = { { 1, 0, 1, 0, 1 }, { 1, 1, 0, 1, 0 }, { 1, 0, 1, 0, 1 }, { 0, 1, 0, 0, 1 } };
        Matrix testMessageMatrix = new Matrix(elements1);
        double probability = 0.2;
        var exception = Assert.Throws<ArgumentException>(() => new Channel(testMessageMatrix, probability));
        Assert.Equal("The message should be sent as a vector (1 row matrix).", exception.Message);
    }

    [Fact]
    public void CheckIfRandomErrorsAppearAsExpected()
    {
        int[,] elements1 = { { 1, 0, 1, 0, 1 } };
        Matrix encodedMessage = new Matrix(elements1);
        double probabilityOfError = 0.5;
        
        // there is a class called Mock that is used to simulate random results

        var mock = new Mock<RandomNumberGenerator>();

        mock.SetupSequence(r => r.GetNewRandomNumber())
            .Returns(0.1) // will be error bit (0.1 < 0.5)
            .Returns(0.6)
            .Returns(0.2) // will be error bit (0.2 < 0.5)
            .Returns(0.7)
            .Returns(0.8);

        Channel channel = new Channel(encodedMessage, probabilityOfError, mock.Object);
        Matrix receivedMessage = channel.ReceivedMessage;
        
        Assert.Equal(0, receivedMessage[0, 0].Value);
        Assert.Equal(0, receivedMessage[0, 1].Value);
        Assert.Equal(0, receivedMessage[0, 2].Value);
        Assert.Equal(0, receivedMessage[0, 3].Value);
        Assert.Equal(1, receivedMessage[0, 4].Value);
        
    }
    
}