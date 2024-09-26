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
    
    
        

}