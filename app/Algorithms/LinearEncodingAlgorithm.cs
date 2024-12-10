namespace app.Algorithms;
using app.Math;



// warning! the encoding algorithm works with only binary digits!

// the length of the original message is considered to be industry-based (known) information, which is not needed
// to be sent through the original vector


/// <summary>
/// This class is the primary encoding algorithm class, used for vector and text encoding.
///
/// Warning! The encoding algorithm works with only binary digits!
/// 
/// </summary>
public class LinearEncodingAlgorithm
{
    private Matrix OriginalMessage { get; }
    public int OriginalMessageLength { get; private set; }
    public Matrix EncodedMessage { get; private set; }
    public Matrix GeneratorMatrix { get; private set; }
    public Field field { get; private set; }
    public int k; // dimensija (how long each divided up part should be)
    
    /// <summary>
    /// Main constructor for the linear encoding algorithm.
    /// Automatically upon creation stores the encoded message in <see cref="EncodedMessage"/>.
    /// </summary>
    /// <param name="originalMessage">Original matrix message which is to be encoded.</param>
    /// <param name="generatorMatrix">Generator matrix used for the encoding.</param>
    /// <param name="dimension">Dimension of the generator matrix.</param>
    /// <param name="n">Code length.</param>
    /// <param name="matrixGenerator">It is possible to specify a custom generator matrix generator
    /// if same random algorithm is to be used for many encodings</param>
    /// <exception cref="ArgumentException">Throws if the message isn't a vector and if dimension count is less or eq. to 0.</exception>
    public LinearEncodingAlgorithm(Matrix originalMessage, Matrix generatorMatrix, int dimension, int n,
        GeneratorMatrixGenerator matrixGenerator = null)
    // a custom matrix generator can be assigned, mostly for mock unit testing
    {
        if (originalMessage.Rows != 1)
        {
            throw new ArgumentException("The original message must be sent as a vector (matrix with one row).");
        }
        
        if (dimension <= 0)
        {
            throw new ArgumentException("The dimension count k cannot be less or equal than 0.");
        }
        this.k = dimension;
        this.OriginalMessage = originalMessage;
        this.OriginalMessageLength = OriginalMessage.Columns;

        // original message length is error-free information!

        if (generatorMatrix == null)
        {
            if (matrixGenerator == null)
            {
                matrixGenerator = new GeneratorMatrixGenerator(new RandomNumberGenerator());
                
            }
            
            this.GeneratorMatrix = matrixGenerator.GenerateMatrix(k, n);
        }
        else
        {
            this.GeneratorMatrix = generatorMatrix;
        }
        
        this.field = originalMessage[0, 0].field;
        
        // call encoding algorithm
        this.EncodedMessage = EncodeMessage();
    }
    
    /// <summary>
    /// Gets the correct message size for encoding (ensures padding).
    /// <remarks>
    /// The problem is that n may not always be divisible by k.
    /// That is why there can be leftover bits that are not in a split message group.
    /// To counter this, additional 0 need to be added to the end of the message so that they can
    /// be encoded with the same generative matrix as all other sections
    /// later on in the decoding process this 0-filling will need to be accounted for
    /// </remarks>
    /// </summary>
    /// <returns>2D <c>int[,]</c> array</returns>
    /// <exception cref="ArgumentException">Throws if dimension count is larger than vector length.</exception>
    public int[,] GetCorrectSizeMessageForEncoding()
    {
        int length = OriginalMessage.Columns; // columns represent the lenght of the vector
        if (k > length)
        {
            throw new ArgumentException("The dimension count k cannot be larger than the vector length.");
        }
        
        // the problem is that n may not always be divisible by k
        // that is why there can be leftover bits that are not in a split message group
        // to counter this, additional 0 need to be added to the end of the message so that they can
        // be encoded with the same generative matrix as all other sections
        // later on in the decoding process this 0-filling will need to be accounted for

        int totalMessageLength;
        if (length % k == 0)
        {
            totalMessageLength = length;
        }
        else
        {
            // since we need to fill the unused space it is calculated like this:
            // our remaining part is length % k, and since the entire section of bits has k bits
            // it means that we will need to add only (k - (length % k) ) bits to the message
            // in order for the entire message to be encoded with the generative matrix
            totalMessageLength = length + (k - (length % k));
        }
        
        // message with the additional space for 0'es
        // totalMessage - a 2d array (matrix), where there is 1 row, and totalMessageLength columns
        // int[,], not FieldElement[,], since the Matrix constructor requires int[,] elements, not FieldElement[,] ones
        int[,] totalMessage = new int[1, totalMessageLength];
        
        // inserting originalMessage into the totalMessage
        for (int i = 0; i < length; ++i)
        {
            totalMessage[0, i] = OriginalMessage[0, i].Value;
        }
        
        // adding zeroes (if required)
        // through testing found that this part is not required, since all values when creating a vector
        // (or a 2d matrix) are initialized to 0 automatically
        
        // for (int i = length; i < totalMessageLength; ++i)
        // {
        //     totalMessage[0, i] = 0;
        // }

        return totalMessage;
    }
    
    /// <summary>
    /// Method to encode a message. Uses properties of the algorithm.
    /// <para>Recommend just using <see cref="EncodedMessage"/> instead.</para>.
    /// </summary>
    /// <returns><c>Matrix</c></returns>
    public Matrix EncodeMessage()
    {
        // the entire message with a correct size for encoding
        // check GetCorrectSizeMessageForEncoding() for more info about message length

        int[,] totalMessage = GetCorrectSizeMessageForEncoding();
        int totalMessageLength = totalMessage.GetLength(1); // number of columns
                                                                    // (length of the message with possibly added 0'es)
                                                            
        int numberOfParts = totalMessageLength / k;
        
        int encodedMessageLengthPerPart = GeneratorMatrix.Columns; // length of the encoded message per part
        int totalEncodedMessageLength = numberOfParts * encodedMessageLengthPerPart;
        int[,] encodedMessageVector = new int[1, totalEncodedMessageLength]; // 1 row, totalEncodedMessageLength columns
                                                                             // of a matrix (meaning a vector in this case)

        // encoding each part of the divided up message
        Dictionary<(Matrix, Matrix), Matrix> multiplicationCache = new Dictionary<(Matrix, Matrix), Matrix>();
        int counter = 0;

        for (int part = 0; part < numberOfParts; ++part)
        {
            int[,] messagePart = new int[1, k]; // 1 row, k columns, since each encodedMessage is k chars long
            for (int column = 0; column < k; ++column)
            {
                messagePart[0, column] = totalMessage[0, part * k + column];
            }


            Matrix partMatrix = new Matrix(messagePart, field.q);
            Matrix encodedPartMatrix = MultiplyCached(multiplicationCache,partMatrix, GeneratorMatrix);

            for (int column = 0; column < encodedMessageLengthPerPart; ++column)
            {
                encodedMessageVector[0, part * encodedMessageLengthPerPart + column] =
                    encodedPartMatrix[0, column].Value;
            }

            ++counter;
            if (part % 10000 == 0)
                Console.WriteLine("Message part " + counter + "/" + numberOfParts + " decoded.");
        }
        
        
        // returning the encoded vector fully merged
        Matrix resultMatrix = new Matrix(encodedMessageVector, field.q);
        return resultMatrix;

    }
    
    /// <summary>
    /// Method to store multiplication results in a cache. Used for to enhance the speed of the encoding algorithm
    /// </summary>
    /// <param name="multiplicationCache">Dictionary which stores the multiplied matrix results.</param>
    /// <param name="a">First matrix</param>
    /// <param name="b">Second matrix</param>
    /// <returns><c>Matrix</c></returns>
    public static Matrix MultiplyCached(Dictionary<(Matrix, Matrix), Matrix> multiplicationCache, Matrix a, Matrix b)
    {
        var key = (a, b);
        if (multiplicationCache.ContainsKey(key))
        {
            return multiplicationCache[key];
        }

        Matrix result = a * b;
        multiplicationCache[key] = result;
        return result;
    }
    
    


}