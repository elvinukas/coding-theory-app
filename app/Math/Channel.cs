namespace app.Math;

public class Channel
{
    // each symbol (or bit in my case) has an independent probabilityOfError 
    public double ProbabilityOfError { get; private set; } // p_e
    public RandomNumberGenerator RandomNumberGenerator { get; set; }
    public Matrix OriginalMessage { get; private set; } // original encoded message (m) without errors
    public Matrix ReceivedMessage { get; private set; } // received encoded message (m') with possible errors
    public static int counter { get; private set; }

    public Channel(Matrix encodedMessage, double probabilityOfError, RandomNumberGenerator? randomNumberGenerator = null)
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

    // introducing errors to a file
    public Channel(string filePath, double probabilityOfError, int k, int n, RandomNumberGenerator? randomNumberGenerator = null)
    {
        
        if (probabilityOfError is > 1 or < 0) // ide recommended this approach, same as || 
        {
            throw new ArgumentException("The probability of an error inside a channel must be between 0 and 1.");
        }

        if (!File.Exists(filePath))
        {
            throw new ArgumentException("Selected file in Channel does not exist.");
        }


        this.ProbabilityOfError = probabilityOfError;
        this.OriginalMessage = null; // when errors are made to a file, no matrix is needed
        if (randomNumberGenerator == null)
        {
            this.RandomNumberGenerator = new RandomNumberGenerator();
        }
        else
        {
            this.RandomNumberGenerator = randomNumberGenerator;
        }
        this.ReceivedMessage = null;
        
        MakeErrorsInFile(filePath, k, n);
    }

    public void MakeErrorsInFile(string filePath, int k, int n)
    {
        byte[] originalBytes = File.ReadAllBytes(filePath);

        byte[] modifiedBytes = ModifyBytes(originalBytes, k, n);
        
        Console.WriteLine("Number of errors inputted: " + counter);
        File.WriteAllBytes(filePath, modifiedBytes);

    }

    private byte[] ModifyBytes(byte[] originalBytes, double k, double n)
    {
        
        byte[] receivedBytes = new byte[originalBytes.Length];

        for (int i = 0; i < originalBytes.Length; ++i)
        {
            if (k == 0)
            {
                if (i <= 54) // 54 bytes is the header for .bmp
                {
                    receivedBytes[i] = originalBytes[i];
                } else
                {
                    receivedBytes[i] = IntroduceErrorsToByte(originalBytes[i]);
                } 
                
            }
            else
            {
                // 54 is the size of the header of .bmp file in bytes, we need to adjust for encoding
                if (i <= (int) ((54+2) * ((double)n / k)))
                {
                    receivedBytes[i] = originalBytes[i];
                } else
                {
                    receivedBytes[i] = IntroduceErrorsToByte(originalBytes[i]);
                } 
            }
            
        }
            
        
        
        

        return receivedBytes;

    }


    private byte IntroduceErrorsToByte(byte originalByte)
    {
        byte receivedByte = originalByte;
        for (int bitPosition = 0; bitPosition < 8; ++bitPosition)
        {
            double randomChanceOfErrorForBit = RandomNumberGenerator.GenerateNumber();
            if (randomChanceOfErrorForBit < ProbabilityOfError)
            {
                // flipping byte
                ++counter;
                receivedByte ^= (byte)(1 << bitPosition);
            }
        }

        return receivedByte;    
    }
    


    // message that gets passed through a channel with a probability of errors
    private Matrix GetReceivedMessage()
    {
        int messageLength = OriginalMessage.Columns;
        int[,] error2DArray = new int[1, messageLength];

        for (int value = 0; value < messageLength; ++value)
        {
            double randomChanceOfErrorForElement = RandomNumberGenerator.GenerateNumber();
            if (randomChanceOfErrorForElement < ProbabilityOfError)
            {
                error2DArray[0, value] = 1;
            }
            
        }
        
        Matrix errorVector = new Matrix(error2DArray); // e

        return OriginalMessage + errorVector; // y = c + e
        
    }
    

    public static Matrix GetSpecifiedNumOfErrorVector(Matrix sentMessage, int numberOfErrors)
    {
        int columns = sentMessage.Columns;
        RandomNumberGenerator randomNumberGenerator = new RandomNumberGenerator();
        Matrix errorVector = new Matrix(new int[1, columns]);
        

        for (int i = 0; i < numberOfErrors; ++i)
        {
            int index = (int) (randomNumberGenerator.GenerateNumber() * columns);
            if (errorVector[0, index].Value == 1)
            {
                --i;
                continue;
            }
            errorVector[0, index] = new FieldElement(1, errorVector[0, index].field);
            
        }

        return errorVector;

    }
    
    
    

}