using System.Runtime.CompilerServices;
using System.Text;
namespace app.Math;

public class Matrix
{
    private FieldElement[,] matrix; // holds a two dimensional array [,]
    public int Rows { get; private set; }
    public int Columns { get; private set; }
    
    // creating an empty matrix
    // standard
    public Matrix(int rows, int columns, int q = 2)
    {
        this.Rows = rows;
        this.Columns = columns;
        matrix = new FieldElement[this.Rows, this.Columns]; // this makes all elements in the matrix group groupelements
        
        
        for (int i = 0; i < this.Rows; ++i)
        {
            for (int j = 0; j < this.Columns; ++j)
            {
                matrix[i, j] = new FieldElement(0, new Field(q));
            }
        }
        
    }

    // converting a 2d array into a GroupElement matrix
    public Matrix(int[,] elements, int q = 2)
    {
        // creating an empty matrix with measurements taken from elements 2d array
        this.Rows = elements.GetLength(0);
        this.Columns = elements.GetLength(1);
        matrix = new FieldElement[Rows, Columns];

        // assigning each matrix element with a specified element from the elements 2d array
        for (int row = 0; row < Rows; ++row)
        {
            for (int column = 0; column < Columns; ++column)
            {
                matrix[row, column] = new FieldElement(elements[row, column], new Field(q));
            }
        }

    }
    
    
    // creating a method for accessing GroupElements from matrix
    public FieldElement this[int i, int j]
    {
        get => matrix[i, j];
        set => matrix[i, j] = value;
    }

    /// matrix operations

    public static bool operator ==(Matrix a, Matrix b)
    {
        if (!(a != b))
        {
            return true;
        }

        return false;
        
    }


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
    public static Matrix MergeVectors(Matrix a, Matrix b)
    {
        if (a.Rows > 1 || b.Rows > 1)
        {
            throw new ArgumentException("Only vectors (matrices with 1 row) can be merged.");
        }

        if (a[0, 0].field != b[0, 0].field)
        {
            throw new ArgumentException("Only vectors (matrices with 1 row) with the same field size q can be merged.");
        }

        int length = a.Columns + b.Columns;
        int[,] newMergedMessageArray = new int[1, length];
        
        // first matrix copy
        for (int column = 0; column < a.Columns; ++column)
        {
            newMergedMessageArray[0, column] = a[0, column].Value;
        }
        
        // second matrix copy
        for (int column = 0; column < b.Columns; ++column)
        {
            newMergedMessageArray[0, a.Columns + column] = b[0, column].Value;
        }

        return new Matrix(newMergedMessageArray, a[0,0].field.q);
        
    }
    
    
    // matrix addition
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
    public static Matrix operator *(Matrix a, Matrix b)
    {
        // checking if matrix multiplication is possible
        if (a.Columns != b.Rows)
        {
            throw new ArithmeticException(
                "Matrix multiplication is not possible. The number of First Matrix columns must equal the number of rows in the Second Matrix");
        }

        if (a.matrix[0, 0].field.q != b.matrix[0, 0].field.q)
        {
            throw new InvalidOperationException(
                "Matrix multiplication is not possible when the different matrixes " +
                "use different field sizes for their field elements.");
        }
        
        // storing the result
        // the matrix multiplication result will be in a shape of a.rows and b.columns
        Matrix result = new Matrix(a.Rows, b.Columns);
        int q = a.matrix[0, 0].field.q;
        for (int row = 0; row < a.Rows; ++row)
        {
            for (int column = 0; column < b.Columns; ++column)
            {
                // multiplication uses the sum as the final result
                FieldElement sumResult = new FieldElement(0, new Field(q));
                
                // follows the rules of matrix multiplication
                // we start from the first matrix and second matrix top left corner and multiply
                // then increase first matrix column by one, and second matrix row by one and multiply
                // continue until every element is multiplied
                for (int i = 0; i < a.Columns; ++i)
                {
                    sumResult += a[row, i] * b[i, column]; // using GroupElement multiplication
                }
    

                result[row, column] = sumResult;
            }
            
        }

        return result;
        
    }

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

}