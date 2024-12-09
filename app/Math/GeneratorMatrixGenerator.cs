namespace app.Math;

// basically,
// vartotojas, pasirinkdamas, kad kompiuteris generuoja generuojancia matrica, prisiima atsakomybe,
// kad tas kodas nevisada gales istaisyti klaidas (arba ju visai neistaisys)
// BET, as pasistengsiu, kad kodas turetu dideli atstuma (jei pavyks)

/// <summary>
/// A generator for standard generator matrices.
/// <remarks>By relying on this class the user takes responsibility, that the generator matrix may not be optimal for encoding.</remarks>
/// </summary>
public class GeneratorMatrixGenerator : IMatrixGen
{

    private readonly INumGen _randomNumberGenerator; 

    // this constructor is mainly for mock unit tests
    /// <summary>
    /// Constructor for the class. Does not automatically generate a matrix.
    /// </summary>
    /// <param name="randomNumberGenerator">Takes in a <c>INumGen</c> interface. Can be easily used for mock tests.</param>
    public GeneratorMatrixGenerator(INumGen randomNumberGenerator)
    {
        _randomNumberGenerator = randomNumberGenerator;
    }

    /// <summary>
    /// Generates a random matrix.
    /// </summary>
    /// <param name="k">Dimension.</param>
    /// <param name="n">Code length.</param>
    /// <returns>Randomized <c>Matrix</c></returns>
    /// <exception cref="ArgumentException"><c>n</c> (the number of bits in each codeword) cannot be smaller or equal
    /// to the number of bits in the original message (<c>k</c>)</exception>
    public Matrix GenerateMatrix(int k, int n)
    {
        if (k >= n)
        {
            throw new ArgumentException("n (the number of bits in each codeword) cannot be smaller or equal to " +
                                        " the number of bits in the original message (k) ");
        }

        int[,] generatorMatrix = new int[k, n];
        
        // we can create a generator matrix (identity matrix) which when multiplied would equal the exact same reslt

        for (int i = 0; i < k; ++i)
        {
            generatorMatrix[i, i] = 1;
            
        }
        
        // the rest of remaining rows are filled with either 0 or 1
        // the message up to k will be encoded using the identity matrix
        // the rest of the encoded bits must be encoded then with random 0 or 1 bits
        // (that's where this following code does)
        for (int row = 0; row < k; ++row)
        {
            
            for (int column = k; column < n; ++column)
            {
                if (_randomNumberGenerator.GenerateNumber() >= 0.5) {
                    generatorMatrix[row, column] = 1;
                } else
                {
                    generatorMatrix[row, column] = 0;
                }
                
            }
            
            
            
            
        }

        return new Matrix(generatorMatrix, 2); // field.q = 2;
        
    }
    
    
    
}