namespace app.Math;

public class Channel
{
    // each symbol (or bit in my case) has an independent probabilityOfError 
    public double ProbabilityOfError { get; private set; } // p_e
    private RandomNumberGenerator RandomNumberGenerator;
    public Matrix OriginalMessage { get; private set; } // original encoded message (m) without errors
    public Matrix ReceivedMessage { get; private set; } // received encoded message (m') with possible errors 

    public Channel(Matrix encodedMessage, double probabilityOfError)
    {
        if (probabilityOfError is > 1 or < 0) // ide recommended this approach, same as || 
        {
            throw new ArgumentException("The probability of an error inside a channel must be between 0 and 1.");
        }

        if (encodedMessage.Rows > 1)
        {
            throw new ArgumentException("The message should be sent as a vector (1 row matrix).");
        }
        
        this.ProbabilityOfError = probabilityOfError;
        this.OriginalMessage = encodedMessage;
        this.RandomNumberGenerator = new RandomNumberGenerator();
        // passing through channel
        this.ReceivedMessage = GetReceivedMessage();
    }


    // message that gets passed through a channel with a probability of errors
    private Matrix GetReceivedMessage()
    {
        int messageLength = OriginalMessage.Columns;
        int[,] originalEncodedMessage = new int[1, messageLength];
        int[,] error2DArray = new int[1, messageLength];

        for (int value = 0; value < messageLength; ++value)
        {
            double randomChanceOfErrorForElement = RandomNumberGenerator.GetNewRandomNumber();
            if (randomChanceOfErrorForElement < ProbabilityOfError)
            {
                var test = OriginalMessage[0, value];
                error2DArray[0, value] = 1;
            }
            
        }

        Matrix originalMessageVector = new Matrix(originalEncodedMessage); // c
        Matrix errorVector = new Matrix(error2DArray); // e

        return originalMessageVector + errorVector; // y = c + e
        
    }
    
    
    

}