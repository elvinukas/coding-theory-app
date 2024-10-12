using Xunit.Abstractions;

namespace tests;
using app.Math;
using app.Algorithms;

public class SimulationUnitTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public SimulationUnitTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Simulation_SendingMessageThroughChannelAndDecode()
    {
        Matrix originalMessage = new Matrix(new int[,]
        {
            {1, 0, 1, 0, 1, 1, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 1}
        });

        Matrix generatorMatrix = new Matrix(new int[,]
        {
            {1, 0, 0, 0, 1, 1, 0},
            {0, 1, 0, 0, 1, 0, 1},
            {0, 0, 1, 0, 1, 1, 1},
            {0, 0, 0, 1, 0, 1, 1}
        });
        
        LinearEncodingAlgorithm linearEncodingAlgorithm = new LinearEncodingAlgorithm(originalMessage, generatorMatrix, 4, 7);
        Matrix encodedMessage = linearEncodingAlgorithm.EncodedMessage;
        Matrix retrievedGeneratorMatrix = linearEncodingAlgorithm.GeneratorMatrix;

        Channel channel = new Channel(encodedMessage, 0.02);
        Matrix encodedMessageWithPossibleErrors = channel.ReceivedMessage;

        Matrix decodedMessage =
            StepByStepDecodingAlgorithm.Decode(retrievedGeneratorMatrix, encodedMessageWithPossibleErrors, linearEncodingAlgorithm.OriginalMessageLength);
        
        _testOutputHelper.WriteLine(originalMessage.ToString());
        _testOutputHelper.WriteLine(decodedMessage.ToString());
        Assert.True(decodedMessage == originalMessage);

        
    }

    [Fact]
    public void Simulation_EncodeAndDecodeText()
    {
        string text = "Išbandome žinutės užkodavimą ir dekodavimą.";
        Matrix convertedMessage = TextConverter.ConvertToBinaryMatrix(text);
        Matrix generatorMatrix = new Matrix(new int[,]
        {
            { 1, 0, 0, 0, 1, 1, 0 },
            { 0, 1, 0, 0, 1, 0, 1 },
            { 0, 0, 1, 0, 1, 1, 1 },
            { 0, 0, 0, 1, 0, 1, 1 }
        });

        LinearEncodingAlgorithm algorithm = new LinearEncodingAlgorithm(convertedMessage, generatorMatrix, 4, 7);
        Matrix encodedMessage = algorithm.EncodedMessage;
        Matrix errorVector = Channel.GetSpecifiedNumOfErrorVector(algorithm.EncodedMessage, 2);
        Matrix receivedMessage = encodedMessage + errorVector;

        Matrix decodedMessage =
            StepByStepDecodingAlgorithm.Decode(generatorMatrix, receivedMessage, algorithm.OriginalMessageLength);

        string retrievedMessage = TextConverter.ConvertToString(decodedMessage);
        _testOutputHelper.WriteLine(retrievedMessage);
        
        Assert.True(text == retrievedMessage);

    }
    
    
}