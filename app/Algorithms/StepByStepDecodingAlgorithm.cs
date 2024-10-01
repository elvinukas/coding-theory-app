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
    public static Matrix GetDecodedMessage(Matrix generatorMatrix, Matrix receivedMessage)
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
        
        
        // temporary return so no compilation errors
        return generatorMatrix;

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
    
    
}