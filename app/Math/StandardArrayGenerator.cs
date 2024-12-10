namespace app.Math;

/// <summary>
/// <remarks>This class should not be used for decoding. It was created a long time ago before I realized the mistake that
/// this is not the correct decoding algorithm for my assignment.
/// <para>It has useful methods and features for other decoder class, that is why it has not been deleted.</para>
/// </remarks>
/// 
/// A table needs to be implemented (standard array) that will help us decode messages.
/// The table is structured in such a way that the first row contains all the valid codewords
/// and each first element in each row is the coset leader (the most likely error).
///
///
/// <br></br>
/// <br></br>
/// <b> If we receive a code that is included in a particular "coset" (as described in literature)
/// then it is likely that the message belongs to a coset leader + error vector </b>
/// 
/// </summary>
///

// wasted much time, this is not the decoding method that I need :)
[Obsolete("Should only be used in addition to other decoding methods")]
public class StandardArrayGenerator
{
    public readonly Matrix GeneratorMatrix;
    public int n { get; set; } // n - the length of each codeword
    public int k { get; set; } // k - original word length of a message (dimension)
    public List<Matrix> Codewords; // list of all the codewords
    public List<Matrix> CosetLeaders; // list of the coset leaders
    public List<List<Matrix>> StandardArray { get; private set; } // the standard array

    /// <summary>
    /// Generates a standard array based on a generator matrix.
    /// </summary>
    /// <param name="generatorMatrix">generator <c>Matrix</c></param>
    /// <exception cref="ArgumentException">Throws if generator matrix is <c>null</c>.</exception>
    public StandardArrayGenerator(Matrix generatorMatrix)
    {
        if (generatorMatrix == null)
        {
            throw new ArgumentException("The generator matrix cannot be null");
        }
        this.GeneratorMatrix = generatorMatrix;
        this.n = GeneratorMatrix.Columns;
        this.k = GeneratorMatrix.Rows;
        this.Codewords = new List<Matrix>();
        this.CosetLeaders = new List<Matrix>();
        this.StandardArray = GenerateStandardArray();
    }

    /// <summary>
    /// Generates a standard array.
    /// </summary>
    /// <returns>A list of a list of matrices.</returns>
    public List<List<Matrix>> GenerateStandardArray()
    {
        Codewords = GenerateAllCodewords();
        CosetLeaders = GenerateCosetLeaders(0);
        StandardArray = new List<List<Matrix>>();
        int columns = Codewords[0].Columns;

        // creating the standard array
        foreach (var cosetLeader in CosetLeaders)
        {
            List<Matrix> rowOfCosets = new List<Matrix>();
            foreach (var codeword in Codewords)
            {
                Matrix standardArrayElement = codeword + cosetLeader;
                rowOfCosets.Add(standardArrayElement);
            }
            StandardArray.Add(rowOfCosets);
            
        }

        return StandardArray;
        
    }

    /// <summary>
    /// This method is used for creating unique syndromes.
    /// Very useful while decoding in <see cref="app.Algorithms.StepByStepDecodingAlgorithm"/>.
    /// </summary>
    /// <param name="parityCheckMatrix">A parity check matrix. This matrix is derived
    /// from the <see cref="app.Algorithms.StepByStepDecodingAlgorithm"/> instructions.</param>
    /// <returns></returns>
    public (List<Matrix>, List<Matrix>, List<int>) GenerateListOfUniqueSyndromes(Matrix parityCheckMatrix)
    {
        int maxUniqueSyndromes = (int)System.Math.Pow(2, n);
        List<Matrix> uniqueSyndromeList = new List<Matrix>();
        List<Matrix> cosetLeaderList = new List<Matrix>();
        List<int> weights = new List<int>();
        
        int weight = 0;
        bool continueLoop = true;
        for (int i = 0; i < maxUniqueSyndromes && continueLoop; ++i)
        {
            List<Matrix> cosetLeaders = GenerateCosetLeaders(weight);

            foreach (Matrix cosetLeader in cosetLeaders)
            {
                //Matrix syndrome = cosetLeader * parityCheckMatrix.Transpose();
                Matrix syndrome = (parityCheckMatrix * cosetLeader.Transpose()).Transpose();

                if (!uniqueSyndromeList.Contains(syndrome))
                {
                    uniqueSyndromeList.Add(syndrome);
                    cosetLeaderList.Add(cosetLeader);

                    int cosetLeaderWeight = GetWeight(cosetLeader);
                    weights.Add(cosetLeaderWeight);
                }

                if (uniqueSyndromeList.Count == maxUniqueSyndromes)
                {
                    continueLoop = false;
                    break;
                }
                
            }

            ++weight;


        }

        return (uniqueSyndromeList, cosetLeaderList, weights);
        
    }


    /// <summary>
    /// Retrieves weight of a matrix (how many <c>1</c> are there).
    /// </summary>
    /// <param name="matrix">Weighed matrix.</param>
    /// <returns><c>int</c></returns>
    public int GetWeight(Matrix matrix)
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


    /// <summary>
    /// Generates the coset leaders of specified weight. Required for the <see cref="app.Algorithms.StepByStepDecodingAlgorithm"/>.
    /// </summary>
    /// <param name="weight">weight of the coset leaders <seealso cref="GetWeight"/>.</param>
    /// <returns></returns>
    public List<Matrix> GenerateCosetLeaders(int weight)
    {
        int lengthOfEncodedMessage = Codewords[0].Columns;
        List<Matrix> cosetLeaders = new List<Matrix>();

        int[] currentLeader = new int[lengthOfEncodedMessage];

        // add the zero vector as the first coset leader when weight is 0
        if (weight == 0)
        {
            int[,] zeroVector = new int[1, lengthOfEncodedMessage];
            Matrix zeroCosetLeader = new Matrix(zeroVector);
            cosetLeaders.Add(zeroCosetLeader);
            return cosetLeaders;
        }

        // for higher weights, generate recursively
        GenerateCosetLeadersRecursive(cosetLeaders, currentLeader, 0, 0, weight);
        return cosetLeaders;
        
    }
    
    /// <summary>
    /// Generating coset leaders up to a certain weight.
    /// </summary>
    /// <param name="maxWeight">The maximum weight up to which coset leaders will be generated. Inclusive.</param>
    /// <returns></returns>
    /// <seealso cref="GetWeight"/>
    /// <seealso cref="GenerateCosetLeaders"/>
    public List<Matrix> GenerateCosetLeadersUpToWeight(int maxWeight)
    {
        int lengthOfEncodedMessage = Codewords[0].Columns;
        List<Matrix> cosetLeaders = new List<Matrix>();

        int[] currentLeader = new int[lengthOfEncodedMessage];

        // add the zero vector as the first coset leader
        int[,] zeroVector = new int[1, lengthOfEncodedMessage];
        Matrix zeroCosetLeader = new Matrix(zeroVector);
        cosetLeaders.Add(zeroCosetLeader);

        // generate coset leaders for each weight from 1 to maxWeight
        for (int weight = 1; weight <= maxWeight; ++weight)
        {
            GenerateCosetLeadersRecursive(cosetLeaders, currentLeader, 0, 0, weight);
        }

        return cosetLeaders;
    }

    
    
    /// <summary>
    /// Recursive method which generates coset leaders.
    /// <para>Quite complicated, looking at implementation recommended.</para>
    /// </summary>
    /// <param name="cosetLeaders">The list of generated coset leaders. Results come here.</param>
    /// <param name="currentLeader">Marks the current leader.</param>
    /// <param name="start">Start weight. Changes each time recursively.</param>
    /// <param name="depth">"How many 1's have been set so far?". Must equal weight to add cosetLeader to the list.</param>
    /// <param name="weight">Weight of a desired coset leader.</param>
    private void GenerateCosetLeadersRecursive(List<Matrix> cosetLeaders, int[] currentLeader, int start, int depth,
        int weight)
    {

        if (depth == weight)
        {
            int[,] cosetLeaderArray = new int[1, currentLeader.Length];
            for (int i = 0; i < currentLeader.Length; ++i)
            {
                cosetLeaderArray[0, i] = currentLeader[i];
            }

            Matrix cosetLeader = new Matrix(cosetLeaderArray);
            cosetLeaders.Add(cosetLeader);
            return;
        }


        for (int i = currentLeader.Length - 1; i >= start; --i)
        {
            currentLeader[i] = 1;
            
            // recursively calling to set the next error position
            GenerateCosetLeadersRecursive(cosetLeaders, currentLeader, i + 1, depth + 1, weight);
            
            // set the current position back to one
            // so that the method can also check other possibilities (we backtrack)
            
            
            // example
            // weight 2 length 5
            // [0, 0, 0, 0, 0]
            // i = 0, [1, 0, 0, 0, 0]
            // then i = 1, [1, 1, 0, 0, 0]
            // since weight == depth, we add it to the list, but this is not the only option!!!!
            // currentLeader[1] = 0;
            // [1, 0, 0, 0, 0]
            // then i increments and currentLeader[2] = 1 -> [1, 0, 1, 0, 0]
            currentLeader[i] = 0;
        }
        
        
        
    }

    /// <summary>
    /// Method to generate all codewords. 
    /// <remarks>Was used to decode vectors.</remarks>
    /// </summary>
    /// <returns>List of matrices.</returns>
    [Obsolete("No longer required, but still used in some unit tests.")]
    private List<Matrix> GenerateAllCodewords()
    {
        
        // first we need to generate all possible messages with length k
        // there are 2^k of those
        double allPossibleMessagesCount = System.Math.Pow(2, k);
       

        List<int[]> possibleMessagesArray = new List<int[]>();
        List<Matrix> possibleCodewordsVector = new List<Matrix>();
        GenerateMessages(new int[k], 0, possibleMessagesArray);

        foreach (var message in possibleMessagesArray)
        {
            int[,] convertedMessageToVertex = new int[1, k];
            for (int i = 0; i < k; ++i)
            {
                convertedMessageToVertex[0, i] = message[i];
            }

            Matrix word = new Matrix(convertedMessageToVertex);
            Matrix codeword = word * GeneratorMatrix;
            
            possibleCodewordsVector.Add(codeword);


        }

        return possibleCodewordsVector;

    }
    
    /// <summary>
    /// Takes in an array and recursively calls the function. Adds each message possibility into the messages list.
    /// </summary>
    /// <param name="current">Current array.</param>
    /// <param name="position">Position within the array.</param>
    /// <param name="messages">The list of messages that are generated.</param>
    [Obsolete("No longer used, only remains in unit testing.")]
    private void GenerateMessages(int[] current, int position, List<int[]> messages)
    {
        if (position == k)
        {
            messages.Add( (int[]) current.Clone());
            return;
        }

        current[position] = 0;
        GenerateMessages(current, position + 1, messages);

        current[position] = 1;
        GenerateMessages(current, position + 1, messages);
        
    }


}