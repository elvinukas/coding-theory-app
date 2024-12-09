namespace app.Math;


/// <summary>
/// This class is used as a channel simulator.
/// <para> Data is passed through this channel and it has a selected probability of flipping a bit in a matrix. </para>
/// </summary>
public class Channel
{
    // each symbol (or bit in my case) has an independent probabilityOfError 
    public double ProbabilityOfError { get; private set; } // p_e
    public RandomNumberGenerator RandomNumberGenerator { get; set; }
    public Matrix OriginalMessage { get; private set; } // original encoded message (m) without errors
    public Matrix ReceivedMessage { get; private set; } // received encoded message (m') with possible errors
    public int counter { get; set; }

    /// <summary>
    /// This is the standard Channel constructor.
    /// <para>Once initiated, the Channel automatically channels the data. The channeled result is in <see cref="ReceivedMessage"/></para>
    /// </summary>
    /// <param name="encodedMessage">The message that will be passed through the channel.</param>
    /// <param name="probabilityOfError">Probability of a single bit flipping. Must be between <c>0</c> and <c>1</c>.</param>
    /// <param name="randomNumberGenerator">Optional argument if desirable to have one specific random generator.</param>
    /// <exception cref="ArgumentException">Throws exception if arguments are incorrect</exception>
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
    
    /// <summary>
    /// This is a Channel constructor for channeling data in file.
    /// <para>Once initiated, the Channel automatically channels the data from file. The channeled result is in the specified file path.</para>
    /// </summary>
    /// <param name="filePath">The file path of data which is to be channeled. Result of channeling is set in the same file.</param>
    /// <param name="probabilityOfError">Probability of a single bit flipping. Must be between <c>0</c> and <c>1</c>.</param>
    /// <param name="k">Dimension.</param>
    /// <param name="n">Code length.</param>
    /// <param name="randomNumberGenerator">Optional argument if desirable to have one specific random generator.</param>
    /// <exception cref="ArgumentException">Throws exception if arguments are incorrect, file does not exist</exception>
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

    /// <summary>
    /// Makes errors in file.
    /// </summary>
    /// <param name="filePath">The file path of data which is to be channeled. Result of channeling is set in the same file.</param>
    /// <param name="k">Dimension.</param>
    /// <param name="n">Code length.</param>
    public void MakeErrorsInFile(string filePath, int k, int n)
    {
        byte[] originalBytes = File.ReadAllBytes(filePath);

        byte[] modifiedBytes = ModifyBytes(originalBytes, k, n);
        
        Console.WriteLine("Number of errors inputted: " + counter);
        File.WriteAllBytes(filePath, modifiedBytes);

    }

    /// <summary>
    /// Modifies the original bytes. Mostly used for accounting the .bmp file header (the first 54 bytes of a file are not channeled).
    /// </summary>
    /// <param name="originalBytes">The bytes that will be channeled.</param>
    /// <param name="k">Dimension.</param>
    /// <param name="n">Code length.</param>
    /// <returns>Modified bytes: <c>byte[]</c></returns>
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

    /// <summary>
    /// Introduces an error(-s) to the byte by flipping them according to the probability provided in the constructor.
    /// </summary>
    /// <param name="originalByte">The byte that will be channeled.</param>
    /// <returns>Modified <c>byte</c></returns>
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
    

    /// <summary>
    /// Retrieving the message that gets passed through a channel with a probability of errors
    /// </summary>
    /// <returns>Channeled <c>Matrix</c></returns>
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
    
    /// <summary>
    /// Static method to introduce a specified number of errors inside a vector.
    /// </summary>
    /// <param name="sentMessage">Message to be modified.</param>
    /// <param name="numberOfErrors"></param>
    /// <returns>Modified <c>Matrix</c></returns>
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