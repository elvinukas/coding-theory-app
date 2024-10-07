namespace app.Math;

public class GeneratorMatrixGenerator
{

    private RandomNumberGenerator RandomNumberGenerator;

    // this constructor is mainly for mock unit tests
    public GeneratorMatrixGenerator(RandomNumberGenerator randomNumberGenerator)
    {
        this.RandomNumberGenerator = randomNumberGenerator;
    }

    public Matrix GenerateGeneratorMatrix(int k, int n)
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
                if (RandomNumberGenerator.GetNewRandomNumber() >= 0.5) {
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