using System.Runtime.CompilerServices;
using System.Text;
using app.Algorithms;

namespace app.Math;

/// <summary>
/// This class represents a mathematical matrix.
/// <para>Can be used to perform mathematical operations on <c>FieldElements</c>.
/// Works with non-standard <c>Fields</c>.</para>
/// </summary>
public class Matrix : ISimilarity<Matrix>
{
    private FieldElement[,] matrix; // holds a two dimensional array [,]
    public int Rows { get; private set; }
    public int Columns { get; private set; }
    
    // creating a matrix filled with zeroes
    // standard
    /// <summary>
    /// Constructor to create a <c>q = 2</c> field matrix filled with zeroes.
    /// </summary>
    /// <param name="rows">Number of rows.</param>
    /// <param name="columns">Number of columns.</param>
    /// <param name="q">Default field parameter set to <c>2</c>.</param>
    public Matrix(int rows, int columns, int q = 2)
    {
        this.Rows = rows;
        this.Columns = columns;
        matrix = new FieldElement[this.Rows, this.Columns]; // this makes all elements in the matrix group groupelements
        
        Field field = new Field(q);
        FieldElement zero = new FieldElement(0, field);
        for (int i = 0; i < this.Rows; ++i)
        {
            for (int j = 0; j < this.Columns; ++j)
            {
                matrix[i, j] = zero;
            }
        }
        
    }

    // converting a 2d array into a FieldElement matrix
    /// <summary>
    /// Constructor converting a 2D array <c>int[,]</c> into a <c>FieldElement</c> object based matrix.
    /// <para>This constructor is the primary constructor to easily create desired matrices.</para>
    /// </summary>
    /// <param name="elements">2D <c>int[,]</c> array designed to replicate a matrix.</param>
    /// <param name="q">Default field parameter set to <c>2</c>.</param>
    public Matrix(int[,] elements, int q = 2)
    {
        // creating an empty matrix with measurements taken from elements 2d array
        this.Rows = elements.GetLength(0);
        this.Columns = elements.GetLength(1);
        matrix = new FieldElement[Rows, Columns];
        Field field = new Field(q);
        
        // binary optimizations if possible
        if (q == 2)
        {
            FieldElement zero = new FieldElement(0, field);
            FieldElement one = new FieldElement(1, field);
            

            for (int row = 0; row < Rows; ++row)
            {
                for (int column = 0; column < Columns; ++column)
                {
                    if (elements[row, column] == 0)
                    {
                        matrix[row, column] = zero;
                    }
                    else if (elements[row, column] == 1)
                    {
                        matrix[row, column] = one;
                    }
                    else
                    {
                        matrix[row, column] = new FieldElement(elements[row, column], field);
                    }
                }
            }

            
        }
        else
        {
            // assigning each matrix element with a specified element from the elements 2d array
            

            for (int row = 0; row < Rows; ++row)
            {
                for (int column = 0; column < Columns; ++column)
                {
                    matrix[row, column] = new FieldElement(elements[row, column], field);
                }
            }
            
        }

        

    }

    // designed only for vectors and mostly reading from .bin files
    public Matrix(byte[] byteArray)
    {
        int[,] binaryVector = new int[1, byteArray.Length * 8]; // * 8 since each byte is 8 bits long
        int column = 0;
        
        // iterating through each byte
        for (int i = 0; i < byteArray.Length; ++i)
        {
            byte textByte = byteArray[i];

            // starting from the last bit of the byte
            for (int bit = 7; bit >= 0; --bit)
            {
                int bitValue = textByte / (int)System.Math.Pow(2, bit);
                binaryVector[0, column] = bitValue % 2;
                ++column;
            }
            
        }

        Matrix newMatrix = new Matrix(binaryVector);
        this.Rows = newMatrix.Rows;
        this.Columns = newMatrix.Columns;
        this.matrix = newMatrix.matrix;
        
    }

    // copy constructor for the .Clone()
    /// <summary>
    /// Copy constructor for <c>.Clone()</c>.
    /// </summary>
    /// <param name="originalMatrix"><c>Matrix</c> which is to be cloned.</param>
    /// <param name="q">Default field parameter set to <c>2</c>.</param>
    public Matrix(Matrix originalMatrix, int q = 2)
    {
        this.Rows = originalMatrix.Rows;
        this.Columns = originalMatrix.Columns;
        this.matrix = new FieldElement[Rows, Columns];

        Field field = new Field(q);
        for (int row = 0; row < Rows; ++row)
        {
            for (int column = 0; column < Columns; ++column)
            {
                matrix[row, column] = new FieldElement(originalMatrix.matrix[row, column].Value, field);
            }
        }

    }
    
    /// <summary>
    /// Clone() method for cloning a matrix
    /// </summary>
    /// <returns><c>Matrix</c></returns>
    public Matrix Clone()
    {
        return new Matrix(this);
    }
    
    
    /// <summary>
    /// Method for accessing FieldElements from matrix
    /// </summary>
    /// <param name="i">Indicating row of matrix.</param>
    /// <param name="j">Indicating column of matrix.</param>
    public FieldElement this[int i, int j]
    {
        get => matrix[i, j];
        set => matrix[i, j] = value;
    }

    
    
    /// <summary>
    /// Overridden Equals() operator. Uses <c>operator ==</c> instead.
    /// </summary>
    /// <param name="obj">Checked object.</param>
    /// <returns><c>bool</c></returns>
    public override bool Equals(object obj)
    {
        if (obj is Matrix matrix)
        {
            return this == matrix; // use the overloaded == operator
        }
        return false;
    }

    /// <summary>
    /// Retrieves the hash code of the Matrix. The hash code is the same for all matrices
    /// so that hashtable uses <c>.Equals()</c> as its comparison
    /// <remarks>Was needed for hashtable comparisons.</remarks>
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return 400; // the hash code is the same for all matrices, so that the hashtable uses .Equals() as its comparison
    }

    /// <summary>
    /// Equality operator for matrices.
    /// </summary>
    /// <param name="a">First matrix</param>
    /// <param name="b">Second matrix</param>
    /// <returns><c>bool</c></returns>
    public static bool operator ==(Matrix a, Matrix b)
    {
        if (!(a != b))
        {
            return true;
        }

        return false;
        
    }


    /// <summary>
    /// Inequality operator for matrices.
    /// </summary>
    /// <param name="a">First matrix</param>
    /// <param name="b">Second matrix</param>
    /// <returns><c>bool</c></returns>
    public static bool operator !=(Matrix a, Matrix b)
    {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
        {
            return false;
        }
        
        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
        {
            return true;
        }
        
        
        if ((a.Rows != b.Rows) || (a.Columns != b.Columns))
        {
            return true;
        }

        for (int row = 0; row < a.Rows; ++row)
        {
            for (int column = 0; column < a.Columns; ++column)
            {
                if (a[row, column] != b[row, column])
                {
                    return true;
                }
                
            }
        }

        return false;
        

    }
    
    
    // merging two vectors into one matrix
    
    /// <summary>
    /// Merges two matrices into one matrix.
    /// </summary>
    /// <param name="a">First matrix</param>
    /// <param name="b">Second matrix</param>
    /// <returns><c>Matrix</c></returns>
    /// <exception cref="ArgumentException">Throws if matrices with not the same amount of rows are inputed,
    /// or field sizes differ.</exception>
    public static Matrix MergeMatrices(Matrix a, Matrix b)
    {
        if (a.Rows != b.Rows)
        {
            throw new ArgumentException("Only matrices with the same amount of rows can be merged.");
        }

        if (a[0, 0].field != b[0, 0].field)
        {
            throw new ArgumentException("Only matrices with the same field size q can be merged.");
        }

        int length = a.Columns + b.Columns;
        int[,] newMergedMessageArray = new int[a.Rows, length];
        
        // first matrix copy
        for (int row = 0; row < a.Rows; ++row)
        {
            for (int column = 0; column < a.Columns; ++column)
            {
                newMergedMessageArray[row, column] = a[row, column].Value; 
            }
        }
        
        
        
        
        // second matrix copy

        for (int row = 0; row < b.Rows; ++row)
        {
           for (int column = 0; column < b.Columns; ++column) 
           {
               newMergedMessageArray[row, a.Columns + column] = b[row, column].Value;
           } 
        }
        

        return new Matrix(newMergedMessageArray, a[0,0].field.q);
        
    }
    
    
    // matrix addition
    
    /// <summary>
    /// Addition operator for matrices.
    /// </summary>
    /// <param name="a">First matrix</param>
    /// <param name="b">Second matrix</param>
    /// <returns><c>Matrix</c></returns>
    public static Matrix operator +(Matrix a, Matrix b)
    {
        // checking if the matrixes can be added together
        if (a.Rows != b.Rows || a.Columns != b.Columns)
        {
            throw new ArithmeticException(
                "Matrixes must have the same number of rows and columns in order to be added.");
        }
        
        if (a.matrix[0, 0].field.q != b.matrix[0, 0].field.q)
        {
            throw new InvalidOperationException(
                "Matrix multiplication is not possible when the different matrixes " +
                "use different field sizes for their field elements.");
        }
        
        // creating a matrix to store a result
        Matrix result = new Matrix(a.Rows, a.Columns);

        for (int row = 0; row < a.Rows; ++row)
        {
            for (int column = 0; column < a.Columns; ++column)
            {
                result[row, column] = a[row, column] + b[row, column]; // this uses the GroupElement
                                                                       // addition with operator overloading
                
            }
        }

        return result;
        
    }
    
    // matrix subtraction
    /// <summary>
    /// Subtraction operator for matrices.
    /// </summary>
    /// <param name="a">First matrix</param>
    /// <param name="b">Second matrix</param>
    /// <returns><c>Matrix</c></returns>
    public static Matrix operator -(Matrix a, Matrix b)
    {
        // checking if matrixes can be subtracted
        if (a.Rows != b.Rows || a.Columns != b.Columns)
        {
            throw new ArithmeticException(
                "Matrixes must have the same number of rows and columns in order to be subtracted.");
        }
        
        if (a.matrix[0, 0].field.q != b.matrix[0, 0].field.q)
        {
            throw new InvalidOperationException(
                "Matrix multiplication is not possible when the different matrixes " +
                "use different field sizes for their field elements.");
        }
        
        // creating a matrix to store the result
        Matrix result = new Matrix(a.Rows, a.Columns);

        for (int row = 0; row < a.Rows; ++row)
        {
            for (int column = 0; column < a.Columns; ++column)
            {
                result[row, column] = a[row, column] - b[row, column]; // this uses the GroupElement
                                                                       // subtraction with operator overloading

            }
            
        }

        return result;


    }
    
    // matrix multiplication
    /// <summary>
    /// Multiplication operator for matrices.
    /// <remarks>Heavily used for encoding/decoding messages.</remarks>
    /// </summary>
    /// <param name="a">First matrix</param>
    /// <param name="b">Second matrix</param>
    /// <returns><c>Matrix</c></returns>
    public static Matrix operator *(Matrix a, Matrix b)
    {
        // checking if matrix multiplication is possible
        if (a.Columns != b.Rows)
        {
            throw new ArithmeticException(
                "Matrix multiplication is not possible. The number of First Matrix columns must equal the number of rows in the Second Matrix");
        }

        int q = a.matrix[0, 0].field.q;
        if (q != b.matrix[0, 0].field.q)
        {
            throw new InvalidOperationException(
                "Matrix multiplication is not possible when the different matrixes " +
                "use different field sizes for their field elements.");
        }

        // storing the result
        // the matrix multiplication result will be in a shape of a.rows and b.columns
        Matrix result = new Matrix(a.Rows, b.Columns);
        Field field = new Field(q);
        FieldElement zero = new FieldElement(0, field);
        FieldElement one = new FieldElement(1, field);
        
        // if the field is 2 (meaning binary matrix), we can make operations using bitwise operators instead of field element ones
        // this could yield better performance
        if (q == 2)
        {
            for (int row = 0; row < a.Rows; ++row)
            {
                for (int column = 0; column < b.Columns; ++column)
                {
                    int sumResult = 0;

                    for (int i = 0; i < a.Columns; ++i)
                    {
                        sumResult ^= (a[row, i].Value & b[i, column].Value); // ^= binary addition plus assignment, & bitwise and
                    }

                    if (sumResult == 0)
                    {
                        result[row, column] = zero;
                    }
                    else
                    {
                        result[row, column] = one;
                    }
                    
                }
            }
            
            return result;
        }
        
        
        // if q != 2, we need to use field element multiplication
        for (int row = 0; row < a.Rows; ++row)
        {
            for (int column = 0; column < b.Columns; ++column)
            {
                // multiplication uses the sum as the final result
                FieldElement sumResult = zero;
                
                // follows the rules of matrix multiplication
                // we start from the first matrix and second matrix top left corner and multiply
                // then increase first matrix column by one, and second matrix row by one and multiply
                // continue until every element is multiplied
                for (int i = 0; i < a.Columns; ++i)
                {
                    if (a[row, i].Value == 0 || b[i, column].Value == 0)
                        continue;
                    sumResult += a[row, i] * b[i, column]; // using FieldElement multiplication
                }
    

                result[row, column] = sumResult;
            }
            
        }

        return result;
        
    }
    
    /// <summary>
    /// Transposes a matrix.
    /// <remarks>Transpose() method is required for the step by step decoding algorithm.</remarks>
    /// </summary>
    /// <returns><c>Matrix</c></returns>
    public Matrix Transpose()
    {
        // a transposed matrix has reversed number of columns and rows, thats why the order is different
        Matrix transposedMatrix = new Matrix(this.Columns, this.Rows);

        for (int row = 0; row < Rows; ++row)
        {
            for (int column = 0; column < Columns; ++column)
            {
                transposedMatrix[column, row] = this[row, column];
            }
        }

        return transposedMatrix;

    }
    
    /// <summary>
    /// Overridden ToString() method to format Matrix into string correctly.
    /// </summary>
    /// <returns><c>string</c></returns>
    public override string ToString()
    {
        StringBuilder result = new StringBuilder();
        for (int row = 0; row < Rows; ++row)
        {
            for (int column = 0; column < Columns; ++column)
            {
                result.Append(matrix[row, column].Value.ToString()).Append(" ");
            }
            result.AppendLine();
        }
        return result.ToString();
    }


    /// <summary>
    /// Calculates the similarity between two matrices.
    /// </summary>
    /// <param name="a">First matrix</param>
    /// <param name="b">Second matrix</param>
    /// <returns><c>double</c> value between <c>0</c> and <c>100</c>.</returns>
    public static double CalculateSimilarity(Matrix a, Matrix b)
    {
        if (a.Rows != b.Rows || a.Columns != b.Columns)
        {
            return 0.0;
        }

        int differenceCounter = 0;
        int totalElements = a.Rows * a.Columns;

        for (int row = 0; row < a.Rows; ++row)
        {
            for (int column = 0; column < a.Columns; ++column)
            {
                if (a[row, column] != b[row, column])
                {
                    ++differenceCounter;
                }
            }
        }

        double similarity = 1.0 - (double)differenceCounter / totalElements;
        return similarity * 100;
    }

}