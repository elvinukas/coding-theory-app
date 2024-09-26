using System.Text;
using System.Text.RegularExpressions;

namespace app.Math;

public class Matrix
{
    private FieldElement[,] matrix; // holds a two dimensional array [,]
    public int rows { get; private set; }
    public int columns { get; private set; }
    
    // creating an empty matrix
    // standard
    public Matrix(int rows, int columns, int q = 2)
    {
        this.rows = rows;
        this.columns = columns;
        matrix = new FieldElement[this.rows, this.columns]; // this makes all elements in the matrix group groupelements
        
        
        for (int i = 0; i < this.rows; ++i)
        {
            for (int j = 0; j < this.columns; ++j)
            {
                matrix[i, j] = new FieldElement(0, new Field(q));
            }
        }
        
    }

    // converting a 2d array into a GroupElement matrix
    public Matrix(int[,] elements, int q = 2)
    {
        // creating an empty matrix with measurements taken from elements 2d array
        this.rows = elements.GetLength(0);
        this.columns = elements.GetLength(1);
        matrix = new FieldElement[rows, columns];

        // assigning each matrix element with a specified element from the elements 2d array
        for (int row = 0; row < rows; ++row)
        {
            for (int column = 0; column < columns; ++column)
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

    // matrix addition
    public static Matrix operator +(Matrix a, Matrix b)
    {
        // checking if the matrixes can be added together
        if (a.rows != b.rows || a.columns != b.columns)
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
        Matrix result = new Matrix(a.rows, a.columns);

        for (int row = 0; row < a.rows; ++row)
        {
            for (int column = 0; column < a.columns; ++column)
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
        if (a.rows != b.rows || a.columns != b.columns)
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
        Matrix result = new Matrix(a.rows, a.columns);

        for (int row = 0; row < a.rows; ++row)
        {
            for (int column = 0; column < a.columns; ++column)
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
        if (a.columns != b.rows)
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
        Matrix result = new Matrix(a.rows, b.columns);
        int q = a.matrix[0, 0].field.q;
        for (int row = 0; row < a.rows; ++row)
        {
            for (int column = 0; column < b.columns; ++column)
            {
                // multiplication uses the sum as the final result
                FieldElement sumResult = new FieldElement(0, new Field(q));
                
                // follows the rules of matrix multiplication
                // we start from the first matrix and second matrix top left corner and multiply
                // then increase first matrix column by one, and second matrix row by one and multiply
                // continue until every element is multiplied
                for (int i = 0; i < a.columns; ++i)
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
        for (int row = 0; row < rows; ++row)
        {
            for (int column = 0; column < columns; ++column)
            {
                result.Append(matrix[row, column].value.ToString()).Append(" ");
            }
            result.AppendLine();
        }
        return result.ToString();
    }

}