using System.Collections;

namespace app.Algorithms;
using app.Math;
using app.Exceptions;

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
    public static Matrix Decode(Matrix generatorMatrix, Matrix receivedMessage, int numberBitLength = 32)
    {
        
        // H = [P^T I_{n-k}]
        
        int k = generatorMatrix.Rows;
        int n = generatorMatrix.Columns;
        int lengthBitSize = numberBitLength; // 32 bits were allocated for storing message length by default
        
        
        
        
        // message cannot be trimmed before decoding!!!
        // it must be decoded fully, then trimmed
        
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
        StandardArrayGenerator standardArrayGenerator = new StandardArrayGenerator(generatorMatrix);
        
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
            List<Matrix> uniqueSyndromes;
            List<Matrix> cosetLeaders;
            List<int> weights;

            (uniqueSyndromes, cosetLeaders, weights) =
                standardArrayGenerator.GenerateListOfUniqueSyndromes(parityCheckMatrix);

            int i = 0;
            //Matrix originalMessageSyndrome = receivedMessagePart * parityCheckMatrix.Transpose();
            Matrix originalMessageSyndrome = (parityCheckMatrix * receivedMessagePart.Transpose()).Transpose();
            int index = uniqueSyndromes.IndexOf(originalMessageSyndrome);
            int originalWeight = weights[index];
            List<Matrix> normalBitFlipList = standardArrayGenerator.GenerateCosetLeadersUpToWeight(originalWeight);
            
            
            while (true)
            {

                Matrix syndrome = (parityCheckMatrix * receivedMessagePart.Transpose()).Transpose();
                int currentIndex = uniqueSyndromes.IndexOf(syndrome);
                int currentWeight = weights[currentIndex];
                
                if (currentWeight == 0)
                {
                    decodedMessage = AppendDecodedMessage(decodedMessage, receivedMessagePart, k);
                    break;
                }
                else
                {
                    //Matrix possibleMessage = receivedMessagePart + normalBitFlipList[i];
                    Matrix possibleMessage = receivedMessagePart + cosetLeaders[i];
                    Matrix possibleMessageSyndrome = (parityCheckMatrix * possibleMessage.Transpose()).Transpose();
                    int syndromeIndex = uniqueSyndromes.IndexOf(possibleMessageSyndrome);
                    
                    if (weights[syndromeIndex] < currentWeight)
                    {
                        receivedMessagePart = possibleMessage.Clone();
                        //i = -1;
                    }

                    ++i;

                }
                
                
            }
            
            
            
            
            
            
            // now that the receivedMessagePart is converted to a matrix (vector)
            // decoding process using the standard array can begin

            // if errors do not exist (meaning syndrome is zero)
            // then we can certainly say that the decoded message is the first k elements of the n-length encoded message
            // if (cosetLeaderWeight == 0)
            // {
            //     decodedMessage = AppendDecodedMessage(decodedMessage, receivedMessagePart, k);
            //
            // }
            // // now if errors do exist (meaning the syndrome is not zero)
            // // we need to create a coset leader
            // // then with that coset leader (error vector) we need to subtract it from the receivedMessage
            // // then we check that result if DoErrorsExistWithSyndrome, and if its zero
            // // then we know our original message = receivedMessage - cosetLeader
            // // we add it to the decodedMessage
            // else
            // {
            //     int weight = 1;
            //
            //     while (true)
            //     {
            //         standardArrayGenerator = new StandardArrayGenerator(generatorMatrix);
            //         List<Matrix> cosetLeaders = standardArrayGenerator.GenerateCosetLeaders(weight);
            //
            //         Matrix bestMessagePart = null;
            //         int minErrorWeight = n; // maximum possible weight
            //         bool foundLeader = false;
            //         
            //         foreach (Matrix cosetLeader in cosetLeaders)
            //         {
            //             Matrix possibleOriginalMessagePart = receivedMessagePart + cosetLeader;
            //             Matrix newSyndrome = possibleOriginalMessagePart * parityCheckMatrix.Transpose();
            //
            //             int errorWeight = GetSyndromeWeight(cosetLeader);
            //             
            //             // compare weights
            //             // finding the best error weight scenario
            //             if (GetSyndromeWeight(newSyndrome) < GetSyndromeWeight(syndrome) && errorWeight < minErrorWeight)
            //             {
            //                 bestMessagePart = possibleOriginalMessagePart;
            //                 minErrorWeight = errorWeight;
            //                 foundLeader = true;
            //                 
            //             }
            //         
            //         }
            //         
            //         
            //
            //         if (foundLeader && bestMessagePart != null)
            //         {
            //             decodedMessage = AppendDecodedMessage(decodedMessage, bestMessagePart, k);
            //             break;
            //         }
            //
            //         // if the decoding process fails, throw exception
            //         if (weight >= n)
            //         {
            //             throw new InvalidOperationException(
            //                 "Something went wrong. Message cannot be decoded (syndrome weight is too large!).");
            //         }
            //
            //         ++weight;
            //     }
            //     
            // }
            
            
        }
        
        int originalMessageLength = RetrieveOriginalMessageLength(decodedMessage, lengthBitSize);
        Matrix fullyDecodedMessage;
        try
        {
            fullyDecodedMessage = TrimDecodedMessage(decodedMessage, originalMessageLength, lengthBitSize);
        }
        catch (DecodingException e) // a decoding exception is thrown if the original message length is impossible to determine
        {
            throw new DecodingException(e.Message);
        }
        
        return fullyDecodedMessage;
        
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


    
    public static bool DoErrorsExistWithSyndrome(Matrix syndrome)
    {
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

    public static int GetWeight(Matrix matrix)
    {
        int weight = 0;
        for (int i = 0; i < matrix.Rows; ++i)
        {
            for (int j = 0; j < matrix.Columns; ++j)
            {
                if (matrix[i, j].Value == 1)
                {
                    ++weight;
                }
            }
        }

        return weight;


    }

    public static int RetrieveOriginalMessageLength(Matrix receivedMessage, int lengthBitSize)
    {
        int messageLength = 0;
        
        for (int i = 0; i < lengthBitSize; ++i)
        {
            messageLength = messageLength * 2 + receivedMessage[0, i].Value;
        }

        return messageLength;
    }

    public static Matrix TrimEncodedMessage(Matrix receivedMessage, int lengthBitSize)
    {
        int totalLength = receivedMessage.Columns;
        
        // remainingLength - how many bits are left after trimming the bits allocated for message size
        int remainingLength = totalLength - lengthBitSize;
        int[,] trimmedMessageArray = new int[1, remainingLength];

        try
        {
            for (int i = 0; i < remainingLength; ++i)
            {
                trimmedMessageArray[0, i] = receivedMessage[0, i + lengthBitSize].Value;
            }

            return new Matrix(trimmedMessageArray, receivedMessage[0, 0].field.q);
        }
        catch (IndexOutOfRangeException e)
        {
            throw new DecodingException(
                "The data has been irrecoverably corrupted. The original message length cannot be determined.", e);
        }
        

    }

    public static Matrix TrimDecodedMessage(Matrix decodedMessage, int originalMessageLength, int lengthBitSize)
    {
        int[,] trimmedMessageArray = new int[1, originalMessageLength];

        for (int i = 0; i < originalMessageLength; ++i)
        {
            trimmedMessageArray[0, i] = decodedMessage[0, i + lengthBitSize].Value;
        }

        return new Matrix(trimmedMessageArray, decodedMessage[0, 0].field.q);
    }

    public static int GetMinimalCodeLength(Matrix generatorMatrix)
    {
        StandardArrayGenerator standardArrayGenerator = new StandardArrayGenerator(generatorMatrix);
        List<Matrix> codewords = standardArrayGenerator.Codewords;

        int minimumCounter = Int32.MaxValue;
        foreach (Matrix codeword in codewords)
        {
            if (GetWeight(codeword) < minimumCounter && GetWeight(codeword) != 0)
            {
                minimumCounter = GetWeight(codeword);
            }
        }


        return minimumCounter;

    }
    
    
    
    
}