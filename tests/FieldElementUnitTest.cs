using System;
using Xunit;
using app.Math;

namespace tests
{
    public class FieldElementUnitTest
    {
        [Fact]
        public void SetFieldSize_ShouldCorrectlyAssignGroupSize()
        {
            Field field = new Field(5);
            Assert.Equal(5, field.q);
        }

        [Fact]
        public void SetFieldSize_ShouldThrowExceptionIfNotPrime()
        {
            var exception = Assert.Throws<ArgumentException>(() => new Field(6));
            Assert.Equal("The group size must be a prime number.", exception.Message);
        }

        [Fact]
        public void SetFieldSize_ShouldThrowExceptionIfLessThan2()
        {
            var exception = Assert.Throws<ArgumentException>(() => new Field(1));
            Assert.Equal("The group size must be a prime number.", exception.Message);
        }

        [Fact]
        public void Constructor_CheckIfConstructorCorrectlyConvertsValue()
        {
            Field field = new Field(5);
            
            FieldElement a = new FieldElement(5, field);
            Assert.Equal(0, a.value);

            FieldElement b = new FieldElement(4, field);
            Assert.Equal(4, b.value);

            FieldElement c = new FieldElement(-2, field);
            Assert.Equal(3, c.value);
        }

        [Fact]
        public void OperatorPlus_CheckIfAdditionIsCorrect()
        {
            Field field = new Field(5);
            
            FieldElement a = new FieldElement(2, field);
            FieldElement b = new FieldElement(3, field);
            FieldElement sum = a + b;
            
            // 2 + 3 => 5 % 5 = 0
            Assert.Equal(0, sum.value);
            
            Assert.Equal(4, (new FieldElement(1, field) + new FieldElement(3, field)).value);
            Assert.Equal(2, (new FieldElement(8, field) + new FieldElement(14, field)).value);
            
            Field field2 = new Field(2);
            Assert.Equal(0, (new FieldElement(8, field2) + new FieldElement(14, field2)).value);
            Assert.Equal(1, (new FieldElement(2, field2) + new FieldElement(15, field2)).value);
        }

        [Fact]
        public void OperatorMultiplication_CheckIfMultiplicationIsCorrect()
        {
            Field field2 = new Field(2);
            Assert.Equal(1, (new FieldElement(1, field2) * new FieldElement(1, field2)).value);
            Assert.Equal(0, (new FieldElement(1, field2) * new FieldElement(0, field2)).value);
            Assert.Equal(0, (new FieldElement(0, field2) * new FieldElement(0, field2)).value);
            
            Field field7 = new Field(7);
            Assert.Equal(5, (new FieldElement(18, field7) * new FieldElement(10, field7)).value);
            Assert.Equal(3, (new FieldElement(5, field7) * new FieldElement(2, field7)).value); 
        }

        [Fact]
        public void OperatorSubtraction_CheckIfSubtractionIsCorrect()
        {
            Field field2 = new Field(2);
            Assert.Equal(0, (new FieldElement(1, field2) - new FieldElement(1, field2)).value);
            Assert.Equal(1, (new FieldElement(1, field2) - new FieldElement(0, field2)).value);
            Assert.Equal(1, (new FieldElement(0, field2) - new FieldElement(1, field2)).value);
            Assert.Equal(0, (new FieldElement(0, field2) - new FieldElement(0, field2)).value);
            
            Field field7 = new Field(7);
            Assert.Equal(6, (new FieldElement(5, field7) - new FieldElement(6, field7)).value);
            Assert.Equal(1, (new FieldElement(18, field7) - new FieldElement(3, field7)).value);
        }
    }
}
