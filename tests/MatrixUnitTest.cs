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
        
        // checking operations

        [Fact]
        public void OperatorPlus_CheckIfMatricesAreAddedCorrectly()
        {
            Field field2 = new Field(2);
            int[,] firstElements = { { 1, 0, 1, 1, 1 }, { 1, 0, 0, 0, 0 }, { 0, 1, 1, 1, 1 }, { 1, 1, 1, 0, 1 } };
            Matrix firstMatrix = new Matrix(firstElements, field2.q);
            int[,] secondElements = { { 1, 1, 0, 0, 0 }, { 0, 0, 1, 0, 0 }, { 1, 0, 1, 1, 1 }, { 0, 1, 0, 0, 0 } };
            Matrix secondMatrix = new Matrix(secondElements, field2.q);
            string expedtedStringOutput = "0 1 1 1 1 \n1 0 1 0 0 \n1 1 0 0 0 \n1 0 1 0 1 \n";
            
            Assert.Equal(expedtedStringOutput, (firstMatrix + secondMatrix).ToString());
            
            // ---
            Field field5 = new Field(5);
            int[,] elements1 = { { 1, 0, 2, 3, 4 }, { 1, 1, 0, 3, 2 }, { 3, 1, 1, 2, 1 }, { 0, 1, 4, 0, 1 } };
            int[,] elements2 = { { 1, 0, 4, 1, 3 }, { 3, 4, 1, 1, 0}, { 0, 4, 1, 1, 0 }, { 1, 1, 2, 3, 0 } };
            Matrix matrix1 = new Matrix(elements1, field5.q);
            Matrix matrix2 = new Matrix(elements2, field5.q);
            expedtedStringOutput = "2 0 1 4 2 \n4 0 1 4 2 \n3 0 2 3 1 \n1 2 1 3 1 \n";
            
            Assert.Equal(expedtedStringOutput, (matrix1 + matrix2).ToString());

        }


        [Fact]
        public void OperatorPlus_CheckIfMatrixAdditionIsAllowed()
        {
            Field field2 = new Field(2);
            
            int[,] firstElements = { { 1, 0, 1, 1, 1 }, { 1, 0, 0, 0, 0 }, { 0, 1, 1, 1, 1 }, { 1, 1, 1, 0, 1 } };
            Matrix firstMatrix = new Matrix(firstElements, field2.q);

            int[,] secondElements = { { 1, 2, 3 }, { 3, 4, 4 } };
            Matrix secondMatrix = new Matrix(secondElements, field2.q);
            
            Assert.Throws<ArithmeticException>(() => firstMatrix + secondMatrix);
            // ------
            int[,] thirdElements = { { 1, 2, 3 }, { 3, 4, 4 } };
            Field field3 = new Field(3);
            Matrix thirdMatrix = new Matrix(thirdElements, field3.q);
            
            Assert.Throws<InvalidOperationException>(() => secondMatrix + thirdMatrix);
            
        }

        [Fact]
        public void OperatorMinus_CheckIfMatricesAreSubtractedCorrectly()
        {
            Field field2 = new Field(2);
            int[,] firstElements = { { 1, 0, 1, 1, 1 }, { 1, 0, 0, 0, 0 }, { 0, 1, 1, 1, 1 }, { 1, 1, 1, 0, 1 } };
            Matrix firstMatrix = new Matrix(firstElements, field2.q);
            int[,] secondElements = { { 1, 1, 0, 0, 0 }, { 0, 0, 1, 0, 0 }, { 1, 0, 1, 1, 1 }, { 0, 1, 0, 0, 0 } };
            Matrix secondMatrix = new Matrix(secondElements, field2.q);
            string expedtedStringOutput = "0 1 1 1 1 \n1 0 1 0 0 \n1 1 0 0 0 \n1 0 1 0 1 \n";
            
            Assert.Equal(expedtedStringOutput, (firstMatrix - secondMatrix).ToString());
            
            // ---
            
            Field field5 = new Field(5);
            int[,] elements1 = { { 1, 0, 2, 3, 4 }, { 1, 1, 0, 3, 2 }, { 3, 1, 1, 2, 1 }, { 0, 1, 4, 0, 1 } };
            int[,] elements2 = { { 1, 0, 4, 1, 3 }, { 3, 4, 1, 1, 0}, { 0, 4, 1, 1, 0 }, { 1, 1, 2, 3, 0 } };
            Matrix matrix1 = new Matrix(elements1, field5.q);
            Matrix matrix2 = new Matrix(elements2, field5.q);
            expedtedStringOutput = "0 0 3 2 1 \n3 2 4 2 2 \n3 2 0 1 1 \n4 0 2 2 1 \n";
            
            Assert.Equal(expedtedStringOutput, (matrix1 - matrix2).ToString());
            
        }

        [Fact]
        public void OperatorMinus_CheckIfMatrixSubtractionIsAllowed()
        {
            Field field2 = new Field(2);
            
            int[,] firstElements = { { 1, 0, 1, 1, 1 }, { 1, 0, 0, 0, 0 }, { 0, 1, 1, 1, 1 }, { 1, 1, 1, 0, 1 } };
            Matrix firstMatrix = new Matrix(firstElements, field2.q);

            int[,] secondElements = { { 1, 2, 3 }, { 3, 4, 4 } };
            Matrix secondMatrix = new Matrix(secondElements, field2.q);
            
            Assert.Throws<ArithmeticException>(() => firstMatrix - secondMatrix);
            // ------
            int[,] thirdElements = { { 1, 2, 3 }, { 3, 4, 4 } };
            Field field3 = new Field(3);
            Matrix thirdMatrix = new Matrix(thirdElements, field3.q);
            
            Assert.Throws<InvalidOperationException>(() => secondMatrix - thirdMatrix);
            
            
        }


        [Fact]
        public void OperatorMultiplication_CheckIfMatricesAreMultipliedCorrectly()
        {
            Field field2 = new Field(2);
            int[,] firstElements = { { 1, 0, 1, 1, 1 }, { 1, 0, 0, 0, 0 }, { 0, 1, 1, 1, 1 }, { 1, 1, 1, 0, 1 } };
            Matrix firstMatrix = new Matrix(firstElements, field2.q);
            int[,] secondElements = { { 1, 1, 0, 0 }, { 0, 1, 0, 0 }, { 0, 1, 1, 1 }, { 1, 0, 0, 0 }, { 1, 0, 1, 1 } };
            Matrix secondMatrix = new Matrix(secondElements, field2.q);
            string expedtedStringOutput = "1 0 0 0 \n1 1 0 0 \n0 0 0 0 \n0 1 0 0 \n";
            
            Assert.Equal(expedtedStringOutput, (firstMatrix * secondMatrix).ToString());
            
            // ---
            
            Field field5 = new Field(5);
            int[,] elements1 = { { 1, 0, 2, 3, 4 }, { 1, 1, 0, 3, 2 }, { 3, 1, 1, 2, 1 }, { 0, 1, 4, 0, 1 }, { 2, 3, 1, 1, 1 } };
            int[,] elements2 = { { 1, 0, 4, 1, 3 }, { 3, 4, 1, 1, 0}, { 0, 4, 1, 1, 0 }, { 1, 1, 2, 3, 0 }, { 4, 3, 2, 1, 0 } };
            Matrix matrix1 = new Matrix(elements1, field5.q);
            Matrix matrix2 = new Matrix(elements2, field5.q);
            expedtedStringOutput = "0 3 0 1 3 \n0 3 0 3 3 \n2 3 0 2 4 \n2 3 2 1 0 \n1 0 1 0 1 \n";
            
            Assert.Equal(expedtedStringOutput, (matrix1 * matrix2).ToString());
        }


        [Fact]
        public void OperatorMultiplication_CheckIfMatrixMultiplicationIsAllowed()
        {
            Field field2 = new Field(2);
            
            int[,] firstElements = { { 1, 0, 1, 1, 1 }, { 1, 0, 0, 0, 0 }, { 0, 1, 1, 1, 1 }, { 1, 1, 1, 0, 1 } };
            Matrix firstMatrix = new Matrix(firstElements, field2.q);

            int[,] secondElements = { { 1, 2, 3 }, { 3, 4, 4 }, { 3, 3, 3 } };
            Matrix secondMatrix = new Matrix(secondElements, field2.q);
            
            Assert.Throws<ArithmeticException>(() => firstMatrix * secondMatrix);
            
            // ------
            int[,] thirdElements = { { 1, 2, 3 }, { 3, 4, 4 }, { 3, 3, 3 } };
            Field field3 = new Field(3);
            Matrix thirdMatrix = new Matrix(thirdElements, field3.q);
            
            Assert.Throws<InvalidOperationException>(() => secondMatrix * thirdMatrix);
            
            
            
        }
        
        
        
    }
}
