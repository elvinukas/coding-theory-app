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

public class StepByStepDecodingAlgorithm
{
    public readonly Matrix GeneratorMatrix;
    public int CodewordLength { get; set; } // n - the length of each codeword
    public int OriginalWordLength { get; set; } // k - original word length of a message (dimension)
    
    
    // firstly, all valid codewords need to be generated and listed
    // for them to be generated, it is crutial that n and k is known
    // the amount of valid messages is 2^k

    public StepByStepDecodingAlgorithm(Matrix generatorMatrix, int n, int k)
    {
        if (generatorMatrix == null || n == null || k == null)
        {
            throw new ArgumentException("No arguments for the step-by-step decoding algorithm can be null.");
        }

        this.GeneratorMatrix = generatorMatrix;
        this.CodewordLength = n;
        this.OriginalWordLength = k;
        
        

    }


}