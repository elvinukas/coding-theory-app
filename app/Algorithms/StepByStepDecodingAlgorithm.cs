using System.Collections;
using app.Services;
using Microsoft.AspNetCore.SignalR;

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


public class StepByStepDecodingAlgorithm
{
    public Matrix GeneratorMatrix { get; private set; }
    public int originalMessageLength { get; private set; }
    public Matrix parityCheckMatrix { get; private set; }
    public int k { get; private set; }
    public int n { get; private set; }
    public StandardArrayGenerator StandardArrayGenerator { get; private set; }
    public static int counter { get; private set; }
    
    public List<Matrix> uniqueSyndromes { get; private set; }
    public List<Matrix> cosetLeaders { get; private set; }
    public List<int> weights { get; private set; }

    private readonly IHubContext<DecodingProgressHub>? _hubContext;


    public StepByStepDecodingAlgorithm(Matrix generatorMatrix, int originalMessageLength,
        IHubContext<DecodingProgressHub>? hubContext = null)
    {
        this.GeneratorMatrix = generatorMatrix;
        this.k = generatorMatrix.Rows;
        this.n = generatorMatrix.Columns;
        this.originalMessageLength = originalMessageLength;
        
        // firstly, parityMatrix P needs to be constructed
        Matrix parityMatrix = RetrieveParityMatrix(generatorMatrix);
        // then, it needs to be transposed
        Matrix transposedParityMatrix = parityMatrix.Transpose();
        // then, parity-check matrix H needs to be constructed
        this.parityCheckMatrix = RetrieveParityCheckMatrix(generatorMatrix, transposedParityMatrix);
        this.StandardArrayGenerator = new StandardArrayGenerator(GeneratorMatrix);
        (uniqueSyndromes, cosetLeaders, weights) =
            StandardArrayGenerator.GenerateListOfUniqueSyndromes(parityCheckMatrix);
        _hubContext = hubContext;


    }

    private Matrix Decode(Matrix receivedMessagePart)
    {
        Matrix decodedMessage = null;
            
        Matrix originalMessageSyndrome = (parityCheckMatrix * receivedMessagePart.Transpose()).Transpose();
        int index = uniqueSyndromes.IndexOf(originalMessageSyndrome);
        int originalWeight = weights[index];
        List<Matrix> normalBitFlipList = StandardArrayGenerator.GenerateCosetLeadersUpToWeight(originalWeight);
            
            
        int i = 0;
        while (true)
        {

            //Matrix syndrome = MultiplyCached(multiplicationCache,parityCheckMatrix, receivedMessagePart.Transpose()).Transpose();
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
                Matrix possibleMessage = receivedMessagePart + normalBitFlipList[i];
                Matrix possibleMessageSyndrome = (parityCheckMatrix * possibleMessage.Transpose()).Transpose();
                int syndromeIndex = uniqueSyndromes.IndexOf(possibleMessageSyndrome);
                    
                if (weights[syndromeIndex] < currentWeight)
                {
                    receivedMessagePart = possibleMessage.Clone();
                    //Console.Write("Error detected and possibly fixed! ");
                    //i = -1;
                }

                ++i;

            }
                
                
        }

        ++counter;
        return decodedMessage;
        
        
    }
    
    public Matrix DecodeMessage(Matrix receivedMessage)
    {
        // H = [P^T I_{n-k}]
        
        int k = GeneratorMatrix.Rows;
        int n = GeneratorMatrix.Columns;
        
        // firstly, parityMatrix P needs to be constructed
        Matrix parityMatrix = RetrieveParityMatrix(GeneratorMatrix);
        
        // then, it needs to be transposed
        Matrix transposedParityMatrix = parityMatrix.Transpose();
        
        // then, parity-check matrix H needs to be constructed
        Matrix parityCheckMatrix = RetrieveParityCheckMatrix(GeneratorMatrix, transposedParityMatrix);
        
        // now syndrome needs to be calculated and the weight of each coset leader (as is in the provided literature)
        
        // firstly, the message needs to be split into parts
        int encodedMessageLength = receivedMessage.Columns;
        int numberOfParts = encodedMessageLength / n;
        Matrix decodedMessage = null;
        StandardArrayGenerator standardArrayGenerator = new StandardArrayGenerator(GeneratorMatrix);
        Dictionary<(Matrix, Matrix), Matrix> multiplicationCache = new Dictionary<(Matrix, Matrix), Matrix>();

        
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

            Matrix messagePart = new Matrix(receivedMessagePartArray);
            Matrix decodedPart = Decode(messagePart);

            if (part % 5000 == 0 | (part - 1 == numberOfParts))
            {
                Console.WriteLine("Decoded: " + (part) + "/" + numberOfParts);
            }

            decodedMessage = AppendDecodedMessage(decodedMessage, decodedPart, k);
        }
        
        
        try
        {
            Matrix fullyDecodedMessage = TrimDecodedMessage(decodedMessage, originalMessageLength);
            return fullyDecodedMessage;

        }
        catch (DecodingException e) // a decoding exception is thrown if the original message length is impossible to determine
        {
            throw new DecodingException(e.Message);
        }
        
        
    }

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
    
    public static Matrix TrimDecodedMessage(Matrix decodedMessage, int originalMessageLength)
    {
        int[,] trimmedMessageArray = new int[1, originalMessageLength];

        for (int i = 0; i < originalMessageLength; ++i)
        {
            trimmedMessageArray[0, i] = decodedMessage[0, i].Value;
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



    public byte[] DecodeFile(string inputFilePath, string outputFilePath)
    {
        int n = GeneratorMatrix.Columns;
        int k = GeneratorMatrix.Rows;
        
        byte[] encodedData = File.ReadAllBytes(inputFilePath);
        BitArray encodedBitArray = new BitArray(encodedData);

        int totalBits = encodedBitArray.Count;
        int numberOfParts = totalBits / n;

        // this is with the padding, we will edit it later
        BitArray decodedBitArray = new BitArray(numberOfParts * k);

       

        //object lockObject = new object();
        Parallel.For(0, numberOfParts, part =>
        {
            int[,] receivedMessagePartArray = new int[1, n];
            for (int i = 0; i < n; i++)
            {
                receivedMessagePartArray[0, i] = encodedBitArray[part * n + i] ? 1 : 0;
            }
        
            Matrix receivedMessagePart = new Matrix(receivedMessagePartArray);
            Matrix decodedPartMatrix = this.Decode(receivedMessagePart);
            
            for (int i = 0; i < k; ++i)
            {
                decodedBitArray[part * k + i] = decodedPartMatrix[0, i].Value == 1;
            }

            if (part % 100000 == 0 || part == numberOfParts)
            {
                lock (Console.Out)
                {
                    Console.WriteLine("Message part " + counter + "/" + numberOfParts + " decoded.");
                    _hubContext?.Clients.All.SendAsync("ReceiveDecodeProgress", counter, numberOfParts);
                }
                
            }
            
            
        });
        
        
        // trimming the bitarray to original message length in bits
        BitArray finalBitArray = new BitArray(originalMessageLength * 8);
        for (int i = 0; i < originalMessageLength * 8; ++i)
        {
            finalBitArray[i] = decodedBitArray[i];
        }

        byte[] finalDecodedBytes = new byte[originalMessageLength];
        finalBitArray.CopyTo(finalDecodedBytes, 0);
        
        File.WriteAllBytes(outputFilePath, finalDecodedBytes);

        return finalDecodedBytes;
        
    }
    
    
    
    
    
    
}