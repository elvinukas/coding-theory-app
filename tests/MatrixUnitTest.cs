using System;
using Xunit;
using app.Math;

namespace tests
{
    public class MatrixUnitTest
    {
        [Fact]
        public void Constructor_CheckIfCorrectEmptyMatrixConstructed()
        {
            Field field = new Field(2); 
            Matrix a = new Matrix(2, 2, field.q); 
            FieldElement zero = new FieldElement(0, field);

            for (int row = 0; row < a.rows; ++row)
            {
                for (int column = 0; column < a.columns; ++column)
                {
                    Assert.Equal(a[row, column].value, zero.value);
                }
            }
        }

        [Fact]
        public void Constructor_CheckIfMatrixWithElementsIsCorrect()
        {
            Field field = new Field(5); 
            int[,] elements = { { 1, 2, 3, 4, 5 }, { 6, 7, 8, 9, 10 } };
            Matrix a = new Matrix(elements, field.q); 

            for (int row = 0; row < a.rows; ++row)
            {
                for (int column = 0; column < a.columns; ++column)
                {
                    Assert.Equal(a[row, column].value, new FieldElement(elements[row, column], field).value);
                }
            }
        }

        [Fact]
        public void Index_GetAndSetElementFromMatrix()
        {
            Field field = new Field(5);
            Matrix matrix = new Matrix(5, 5, field.q);
            
            FieldElement expectedElement = new FieldElement(6, field);
            matrix[2, 4] = expectedElement;
            
            FieldElement actualElement = matrix[2, 4];
        
            Assert.Equal(expectedElement.value, actualElement.value);
        }
    
        [Fact]
        public void ToString_CheckIfCorrectMatrix()
        {
            Field field2 = new Field(2); 
            int[,] elements = { { 1, 2 }, { 3, 4 } };
            Matrix matrix = new Matrix(elements, field2.q); 
            string expectedStringOutput = "1 0 \n1 0 \n";
        
            Assert.Equal(expectedStringOutput, matrix.ToString());

            Field field5 = new Field(5); 
            int[,] newElements = { { 1, 2, 3, 4, 5}, { 3, 4, 5, 6, 6 } };
            Matrix newMatrix = new Matrix(newElements, field5.q); 
            expectedStringOutput = "1 2 3 4 0 \n3 4 0 1 1 \n";
        
            Assert.Equal(expectedStringOutput, newMatrix.ToString());
        }
    }
}
