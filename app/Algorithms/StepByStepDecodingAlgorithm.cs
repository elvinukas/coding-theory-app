namespace app.Algorithms;
using app.Math;

/// <summary>
/// The step-by-step decoding algorithm depends on matrix transposition, parity matrix and the generator matrix structure.
/// <br></br>
/// Each generator matrix has an identity matrix within it and some additional bits for adapting the message to the desired length.
/// Now, the parity matrix H (the matrix which is used to check the encoded codeword) is generated like this:
/// G * H^T = 0, where T is transposition applied to the matrix.
/// Now H is constructed by using transposition on the additional bits in the generator matrix for adapting the message to the desired length.
/// </summary>


public static class StepByStepDecodingAlgorithm
{
    public static Matrix Decode(Matrix generatorMatrix, Matrix receivedMessage)
    {
        
        // H = [P^T I_{n-k}]
        
        int k = generatorMatrix.Rows;
        int n = generatorMatrix.Columns;

        // firstly, parityMatrix P needs to be constructed
        Matrix parityMatrix = RetrieveParityMatrix(generatorMatrix);
        
        // then, it needs to be transposed
        Matrix transposedParityMatrix = parityMatrix.Transpose();
        
        // then, parity-check matrix H needs to be constructed
        Matrix parityCheckMatrix = RetrieveParityCheckMatrix(generatorMatrix, transposedParityMatrix);
        
        // now syndrome needs to be calculated and the weight of each coset leader (as is in the provided literature)
        
        // firstly, the message needs to be split into parts
        int encodedMessageLength = receivedMessage.Columns;
        int numberOfParts = encodedMessageLength / n;
        Matrix decodedMessage = null;

        for (int part = 0; part < numberOfParts; ++part)
        {
            int[,] receivedMessagePartArray = new int[1, n];

            for (int column = 0; column < n; ++column)
            {
                // receivedMessage[0, part * n + column] is a FieldElement object
                // therefore .Value needs to be received so that the int[,] array can be filled
                // matrix object can be created using the int[,] argument, not the FieldElement[,] argument,
                // so that is why its easier just to use ints, later on when the matrix is created
                // they are automatically turned to field elements
                receivedMessagePartArray[0, column] = receivedMessage[0, part * n + column].Value;
            }

            Matrix receivedMessagePart = new Matrix(receivedMessagePartArray);
            
            // now that the receivedMessagePart is converted to a matrix (vector)
            // decoding process using the standard array can begin

            // if errors do not exist (meaning syndrome is zero)
            // then we can certainly say that the decoded message is the first k elements of the n-length encoded message
            if (!DoErrorsExistWithSyndrome(receivedMessagePart, parityCheckMatrix))
            {
                decodedMessage = AppendDecodedMessage(decodedMessage, receivedMessagePart, k);

            }
            // now if errors do exist (meaning the syndrome is not zero)
            // we need to create a coset leader
            // then with that coset leader (error vector) we need to subtract it from the receivedMessage
            // then we check that result if DoErrorsExistWithSyndrome, and if its zero
            // then we know our original message = receivedMessage - cosetLeader
            // we add it to the decodedMessage
            else
            {
                int weight = 1;

                while (true)
                {
                    StandardArrayGenerator standardArrayGenerator = new StandardArrayGenerator(generatorMatrix);
                    List<Matrix> cosetLeaders = standardArrayGenerator.GenerateCosetLeaders(weight);

                    bool foundLeader = false;
                    
                    foreach (Matrix cosetLeader in cosetLeaders)
                    {
                        Matrix possibleOriginalMessagePart = receivedMessagePart - cosetLeader;
                        // if syndrome is 0, then its ok! we add the received message part to the decoded messages
                        if (!DoErrorsExistWithSyndrome(possibleOriginalMessagePart, parityCheckMatrix))
                        {
                            decodedMessage = AppendDecodedMessage(decodedMessage, possibleOriginalMessagePart, k);
                            foundLeader = true;
                            break;
                    
                        }
                    
                    }

                    if (foundLeader)
                    {
                        break;
                    }

                    // if the decoding process fails, throw exception
                    if (weight >= n)
                    {
                        throw new InvalidOperationException(
                            "Something went wrong. Message cannot be decoded (syndrome weight is too large!).");
                    }

                    ++weight;
                }
                
            }
            
            
        }

        return decodedMessage;



    }

    private static Matrix AppendDecodedMessage(Matrix decodedMessage, Matrix receivedMessagePart, int k)
    {
        int[,] originalMessagePartElements = new int[1, k];
        Matrix originalMessagePart = new Matrix(originalMessagePartElements);
                
        // only first k elements will be the original message content
        for (int column = 0; column < k; ++column)
        {
            originalMessagePart[0, column] = new FieldElement(receivedMessagePart[0, column].Value,
                receivedMessagePart[0, column].field);
        }
                
        if (decodedMessage == null)
        {
            decodedMessage = originalMessagePart.Clone();
        }
        else
        {
            decodedMessage = Matrix.MergeMatrices(decodedMessage, originalMessagePart);
        }

        return decodedMessage;

    }
    
    


    public static Matrix RetrieveParityMatrix(Matrix generatorMatrix)
    {
        // this generates an empty parity matrix template
        // it will basically be this:
        // G = 
        // {1, 0, 0, 0, 1}
        // {0, 1, 0, 0, 1}
        // {0, 0, 1, 1, 1}
        
        // so the P (parity matrix) is:
        // {0, 1}
        // {0, 1}
        // {1, 1}
        int k = generatorMatrix.Rows;
        int n = generatorMatrix.Columns;
        
        Matrix parityMatrix = new Matrix(k, n - k);

        // the row amount is the same, its the columns that are different
        for (int row = 0; row < k; ++row)
        {
            // important - starting from 0, but we are iterating through the parityMatrix, which is smaller in size!
            for (int column = 0; column < (n - k); ++column)
            {
                parityMatrix[row, column] = generatorMatrix[row, column + k];
                
            }
        }

        return parityMatrix;

    }

    public static Matrix RetrieveParityCheckMatrix(Matrix generatorMatrix, Matrix transposedParityMatrix)
    {
        // H = [P^T I_{n-k}]
        
        // first the identity matrix needs to be generated
        
        int k = generatorMatrix.Rows;
        int n = generatorMatrix.Columns;
        // identityMatrix is the size of (n - k) x (n - k)
        int[,] identityMatrixElements = new int[n - k, n - k];

        for (int i = 0; i < n - k; ++i)
        {
            identityMatrixElements[i, i] = 1;
        }

        Matrix identityMatrix = new Matrix(identityMatrixElements);

        return Matrix.MergeMatrices(transposedParityMatrix, identityMatrix);
        
    }


    
    public static bool DoErrorsExistWithSyndrome(Matrix receivedMessage, Matrix parityCheckMatrix)
    {
        Matrix syndrome = receivedMessage * parityCheckMatrix.Transpose();
        FieldElement zero = new FieldElement(0, new Field(2));
        
        for (int row = 0; row < syndrome.Rows; ++row)
        {
            for (int column = 0; column < syndrome.Columns; ++column)
            {
                if (syndrome[row, column] != zero)
                {
                    return true;
                }
            }
        }

        return false;

    }
    
    
}