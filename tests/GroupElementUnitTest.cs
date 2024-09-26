using System.Text.RegularExpressions;
using app.Math;
namespace tests;

public class GroupElementUnitTest
{
    [Fact]
    public void SetGroupSize_ShouldCorrectlyAssignGroupSize()
    {
        GroupElement.SetGroupSize(5);
        Assert.Equal(5, GroupElement.q);
        
    }

    [Fact]
    public void SetGroupSize_ShouldThrowExceptionIfNotPrime()
    {
        var exception = Assert.Throws<ArgumentException>(() => GroupElement.SetGroupSize(6));
        Assert.Equal("The group size must be a prime number.", exception.Message);
    }

    [Fact]
    public void SetGroupSize_ShouldThrowExceptionIfLessThan2()
    {
        var exception = Assert.Throws<ArgumentException>(() => GroupElement.SetGroupSize(1));
        Assert.Equal("The group size must be a prime number.", exception.Message);
    }

    [Fact]
    public void Constructor_CheckIfConstructorCorrectlyConvertsValue()
    {
        GroupElement.SetGroupSize(5);
        GroupElement a = new GroupElement(5);
        Assert.Equal(0, a.value);

        GroupElement b = new GroupElement(4);
        Assert.Equal(4, b.value);

        GroupElement c = new GroupElement(-2);
        Assert.Equal(3, c.value);
    }

    [Fact]
    public void OperatorPlus_CheckIfAdditionIsCorrect()
    {
        GroupElement.SetGroupSize(5);
        GroupElement a = new GroupElement(2);
        GroupElement b = new GroupElement(3);
        GroupElement sum = a + b;
        
        // 2 + 3 => 5 % 5 = 0
        Assert.Equal(0, sum.value);
        
        Assert.Equal(4, (new GroupElement(1) + new GroupElement(3)).value);
        Assert.Equal(2, (new GroupElement(8) + new GroupElement(14)).value);
        
        GroupElement.SetGroupSize(2);
        Assert.Equal(0, (new GroupElement(8) + new GroupElement(14)).value);
        Assert.Equal(1, (new GroupElement(2) + new GroupElement(15)).value);
        
    }

    [Fact]
    public void OperatorMultiplication_CheckIfMultiplicationIsCorrect()
    {
        GroupElement.SetGroupSize(2);
        Assert.Equal(1, (new GroupElement(1) * new GroupElement(1)).value);
        Assert.Equal(0, (new GroupElement(1) * new GroupElement(0)).value);
        Assert.Equal(0, (new GroupElement(0) * new GroupElement(0)).value);
        
        GroupElement.SetGroupSize(7);
        Assert.Equal(5, (new GroupElement(18) * new GroupElement(10)).value);
        Assert.Equal(3, (new GroupElement(5) * new GroupElement(2)).value); 
        
    }

    [Fact]
    public void OperatorSubtraction_CheckIfSubtractionIsCorrect()
    {
        GroupElement.SetGroupSize(2);
        Assert.Equal(0, (new GroupElement(1) - new GroupElement(1)).value);
        Assert.Equal(1, (new GroupElement(1) - new GroupElement(0)).value);
        Assert.Equal(1, (new GroupElement(0) - new GroupElement(1)).value);
        Assert.Equal(0, (new GroupElement(0) - new GroupElement(0)).value);
        
        GroupElement.SetGroupSize(7);
        Assert.Equal(6, (new GroupElement(5) - new GroupElement(6)).value);
        Assert.Equal(1, (new GroupElement(18) - new GroupElement(3)).value);
        
        
    }
}