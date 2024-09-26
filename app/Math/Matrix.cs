using System.Text.RegularExpressions;

namespace app.Math;

public class Matrix
{
    private GroupElement[,] matrix; // holds a two dimensional array [,]
    private int rows;
    private int columns;
    
    // creating an empty matrix
    public Matrix(int rows, int columns)
    {
        this.rows = rows;
        this.columns = columns;
        matrix = new GroupElement[this.rows, this.columns]; // this makes all elements in the matrix groupelements
    }

    // converting a 2d array into a GroupElement matrix
    public Matrix(int[,] elements)
    {
        // creating an empty matrix with measurements taken from elements 2d array
        this.rows = elements.GetLength(0);
        this.columns = elements.GetLength(1);
        matrix = new GroupElement[rows, columns];

        // assigning each matrix element with a specified element from the elements 2d array
        for (int row = 0; row < rows; ++row)
        {
            for (int column = 0; column < columns; ++column)
            {
                matrix[row, column] = new GroupElement(elements[row, column]);
            }
        }

    }
    
    // creating a method for accessing GroupElements from matrix
    public GroupElement this[int i, int j]
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
        
        // storing the result
        // the matrix multiplication result will be in a shape of a.rows and b.columns
        Matrix result = new Matrix(a.rows, b.columns);

        for (int row = 0; row < a.rows; ++row)
        {
            for (int column = 0; column < b.columns; ++column)
            {
                // multiplication uses the sum as the final result
                GroupElement sumResult = new GroupElement(0);
                
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

    public string ToString()
    {
        string result = "";
        for (int row = 0; row < rows; ++row)
        {
            for (int column = 0; column < columns; ++column)
            {
                result += (matrix[row, column].value).ToString() + "\t";
            }
        }

        return result;

    }

}