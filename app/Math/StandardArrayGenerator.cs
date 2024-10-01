namespace app.Math;

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
///

// wasted much time, this is not the decoding method that I need :)

public class StandardArrayGenerator
{
    public readonly Matrix GeneratorMatrix;
    public int n { get; set; } // n - the length of each codeword
    public int k { get; set; } // k - original word length of a message (dimension)
    public List<Matrix> Codewords; // list of all the codewords
    public List<Matrix> CosetLeaders; // list of the coset leaders
    public List<List<Matrix>> StandardArray { get; private set; } // the standard array


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


    public List<Matrix> GenerateCosetLeaders(int weight)
    {
        int lengthOfEncodedMessage = Codewords[0].Columns;
        List<Matrix> cosetLeaders = new List<Matrix>();

        int[] currentLeader = new int[lengthOfEncodedMessage];
        if (weight == 0)
        {
            int[,] zeroVector = new int[1, lengthOfEncodedMessage];
            Matrix zeroCosetLeader = new Matrix(zeroVector);
            cosetLeaders.Add(zeroCosetLeader);
            
            // this is for the standard array creation, later on probably will need to delete
            GenerateCosetLeadersRecursive(cosetLeaders, currentLeader, 0, 0, weight + 1);
            return cosetLeaders;
        }

        // if weight is not 0, coset leaders need to be generated recursively using the given weight

        

        GenerateCosetLeadersRecursive(cosetLeaders, currentLeader, 0, 0, weight);
        return cosetLeaders;
        
    }

    
    // depth - "how many 1's have been set so far". must equal weight to add cosetLeader to the list
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


        for (int i = start; i < currentLeader.Length; ++i)
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

    // what this method does is it takes in array and recursively calls the function
    // and adds each possibility into the messages list
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