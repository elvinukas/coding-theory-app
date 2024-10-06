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
        
        LinearEncodingAlgorithm linearEncodingAlgorithm = new LinearEncodingAlgorithm(originalMessage, null, 5, 8, numberBitLength: 8);
        Matrix encodedMessage = linearEncodingAlgorithm.EncodedMessage;
        Matrix retrievedGeneratorMatrix = linearEncodingAlgorithm.GeneratorMatrix;

        Channel channel = new Channel(encodedMessage, 0.05);
        Matrix encodedMessageWithPossibleErrors = channel.ReceivedMessage;

        Matrix decodedMessage =
            StepByStepDecodingAlgorithm.Decode(retrievedGeneratorMatrix, encodedMessageWithPossibleErrors, numberBitLength: 8);
        
        _testOutputHelper.WriteLine(originalMessage.ToString());
        _testOutputHelper.WriteLine(decodedMessage.ToString());
        Assert.True(decodedMessage == originalMessage);

        
    }
    
    
}