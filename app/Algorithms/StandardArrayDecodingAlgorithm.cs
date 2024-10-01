namespace app.Algorithms;
using app.Math;

/// <summary>
/// A table needs to be implemented (standard array) that will help us decode messages.
/// The table is structured in such a way that the first row contains all the valid codewords
/// and each first element in each row is the coset leader (the most likely error).
/// <br></br>
/// <br></br>
/// <b> If we receive a code that is included in a particular "coset" (as described in literature)
/// then it is likely that the message belongs to a coset leader + error vector </b>
/// 
/// </summary>

public static class StandardArrayDecodingAlgorithm
{
    // firstly, all valid codewords need to be generated and listed
    // for them to be generated, it is crutial that n and k is known
    // the amount of valid messages is 2^k

    public static Matrix GetOriginalEncodedMessage(Matrix generatorMatrix, Matrix receivedMessage)
    {
        int k = generatorMatrix.Rows;
        int n = generatorMatrix.Columns;

        StandardArrayGenerator standardArrayGenerator = new StandardArrayGenerator(generatorMatrix);
        List<List<Matrix>> standardArray = standardArrayGenerator.StandardArray;
        
        // first, the received message needs to be split so that each encoded message part is n length
        int encodedMessageLength = receivedMessage.Columns;
        int numberOfParts = encodedMessageLength / n;

        int[,] decodedMessageArray = new int[1, k * numberOfParts]; // this is where the original message gets stored

        Matrix originalEncodedMessage = null;
        
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
            
            //------------------------------------------------------------------------------
            // Error-fixing of the encoded message
            //------------------------------------------------------------------------------
            
            // the message needs to be run through every row
            // when the message is found, the row's coset leader must be found
            // and the encoded message with possible errors needs to be subtracted by the coset leader
            // and this is how the original encoded message is found

            bool found = false;
            foreach (var row in standardArray)
            {
                foreach (var cosetElement in row)
                {
                    if (receivedMessagePart == cosetElement)
                    {
                        Matrix cosetLeader = row[0];
                        // y = c + e
                        // c = y - e
                        Matrix result = receivedMessagePart - cosetLeader;
                        if (originalEncodedMessage == null)
                        {
                            // avoiding copy operator
                            originalEncodedMessage = result.Clone();
                        }
                        else
                        {
                            originalEncodedMessage = Matrix.MergeVectors(originalEncodedMessage, result);
                        }
                        
                        // no point in continuing the search if the vector is already found
                        found = true;
                        break;
                        
                    }
                    
                }
                // no point in continuing the search if the vector is already found
                // completely breaking out of the loop
                if (found)
                {
                    break;
                }
                
            }
            
        }
        
        return originalEncodedMessage;

    }

   


}