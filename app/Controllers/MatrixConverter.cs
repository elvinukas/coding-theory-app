using app.Math;

namespace app.Controllers;


/// <summary>
/// This class acts as a converter for matrices of many types (2D list matrices, <see cref="Matrix"/> matrices and more)
/// </summary>
public class MatrixConverter
{
    /// <summary>
    /// Converts a 2D list matrix into a <c>int[,]</c> array.
    /// The frontend can easily support a 2D list sent through JSON, but <see cref="Matrix"/> constructor can only take in
    /// <c>int[,]</c> data, so for easy creation this method can be used.
    /// </summary>
    /// <param name="matrix">2D LIST of a matrix represented by <c>int</c> values</param>
    /// <returns><c>int[,]</c></returns>
    public static int[,] ConvertToIntArray(List<List<int>> matrix)
    {
        int rows = matrix.Count;
        int cols = matrix[0].Count;
        int[,] matrixArray = new int[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                matrixArray[i, j] = matrix[i][j];
            }
        }

        return matrixArray;
    }
    
    /// <summary>
    /// Converts a <see cref="Matrix"/> to a 2D LISt
    /// </summary>
    /// <param name="matrix"><see cref="Matrix"/></param>
    /// <returns>2D List</returns>
    public static List<List<int>> ConvertTo2DList(Matrix matrix)
    {
        List<List<int>> list = new List<List<int>>();

        for (int i = 0; i < matrix.Rows; ++i)
        {
            list.Add(new List<int>());
            for (int j = 0; j < matrix.Columns; ++j)
            {
                list[i].Add(matrix[i, j].Value);
            }
        }

        return list;


    }
}