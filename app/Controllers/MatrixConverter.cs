using app.Math;

namespace app.Controllers;

public class MatrixConverter
{
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