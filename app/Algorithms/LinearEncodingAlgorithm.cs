namespace app.Algorithms;
using app.Math;

public class LinearEncodingAlgorithm
{
    public Matrix originalMessage { get; private set; }
    public int originalMessageLength { get; private set; }
    public Matrix encodedMessage { get; private set; }
    public Matrix generatorMatrix { get; private set; }
    public Field field { get; private set; }
    public int k; // dimensija (how long each divided up part should be)

    public LinearEncodingAlgorithm(Matrix originalMessage, Matrix generatorMatrix, int dimension)
    {
        if (originalMessage.rows != 1)
        {
            throw new ArgumentException("The original message must be sent as a vector (matrix with one row).");
        }
        
        this.originalMessage = originalMessage;
        this.generatorMatrix = generatorMatrix;
        this.field = originalMessage[0, 0].field;

        if (dimension <= 0)
        {
            throw new ArgumentException("The dimension count k cannot be less or equal than 0.");
        }
        this.k = dimension;
        
        // call encoding algorithm
        (this.encodedMessage, originalMessageLength) = EncodeMessage();
        
    }

    public int[,] GetCorrectSizeMessageForEncoding()
    {
        int length = originalMessage.columns; // columns represent the lenght of the vector
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
            totalMessage[0, i] = originalMessage[0, i].value;
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
    
    public (Matrix, int) EncodeMessage()
    {
        // the entire message with a correct size for encoding
        // check GetCorrectSizeMessageForEncoding() for more info about message length

        int[,] totalMessage = GetCorrectSizeMessageForEncoding();
        int totalMessageLength = totalMessage.GetLength(1); // number of columns
                                                                    // (length of the message with possibly added 0'es)
                                                            
        int numberOfParts = totalMessageLength / k;

        int encodedMessageRows = generatorMatrix.rows; // the encoded message will have the same amount of rows
                                                       // as the generator matrix

        int encodedMessageLengthPerPart = generatorMatrix.columns; // length of the encoded message per part
        int totalEncodedMessageLength = numberOfParts * encodedMessageLengthPerPart;
        int[,] encodedMessageVector = new int[1, totalEncodedMessageLength]; // 1 row, totalEncodedMessageLength columns
                                                                             // of a matrix (meaning a vector in this case)
                                                                             
        // encoding each part of the divided up message
        for (int part = 0; part < numberOfParts; ++part)
        {
            int[,] messagePart = new int[1, k]; // 1 row, k columns, since each encodedMessage is k chars long
            for (int column = 0; column < k; ++column)
            {
                messagePart[0, column] = totalMessage[0, part * k + column];
            }


            Matrix partMatrix = new Matrix(messagePart, field.q);
            Matrix encodedPartMatrix = partMatrix * generatorMatrix;

            for (int column = 0; column < encodedMessageLengthPerPart; ++column)
            {
                encodedMessageVector[0, part * encodedMessageLengthPerPart + column] =
                    encodedPartMatrix[0, column].value;
            }

        }
        
        // returning the encoded vector fully merged
        Matrix resultMatrix = new Matrix(encodedMessageVector, field.q);
        return (resultMatrix, originalMessage.columns);

    }
    
    


}