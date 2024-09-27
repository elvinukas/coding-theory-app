namespace app.Math;

public class Channel
{
    // each symbol (or bit in my case) has an independent probabilityOfError 
    public double ProbabilityOfError { get; private set; } // p_e
    public RandomNumberGenerator RandomNumberGenerator { get; set; }
    public Matrix OriginalMessage { get; private set; } // original encoded message (m) without errors
    public Matrix ReceivedMessage { get; private set; } // received encoded message (m') with possible errors 

    public Channel(Matrix encodedMessage, double probabilityOfError, RandomNumberGenerator randomNumberGenerator = null)
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
        // this is used for mock unit testing. if the "mocked" randomNumberGenerator is provided, use that instead.
        this.RandomNumberGenerator = randomNumberGenerator ?? new RandomNumberGenerator();
        // passing through channel
        this.ReceivedMessage = GetReceivedMessage();
    }


    // message that gets passed through a channel with a probability of errors
    private Matrix GetReceivedMessage()
    {
        int messageLength = OriginalMessage.Columns;
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
        
        Matrix errorVector = new Matrix(error2DArray); // e

        return OriginalMessage + errorVector; // y = c + e
        
    }
    
    
    

}