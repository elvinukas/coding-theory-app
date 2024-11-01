using app.Math;
using app.Algorithms;
namespace tests;

public class TextConverterUnitTests
{
    [Fact]
    public void ConvertToBinary_CheckIfConversionIsCorrect()
    {
        string message = "labas";
        Matrix convertedMessage = TextConverter.ConvertToBinaryMatrix(message);

        Matrix expectedConvertedMessage = new Matrix(new int[,]
        {
            {0,1,1,0,1,1,0,0,0,1,1,0,0,0,0,1,0,1,1,0,0,0,1,0,0,1,1,0,0,0,0,1,0,1,1,1,0,0,1,1}
        });

        Assert.True(convertedMessage == expectedConvertedMessage);
    }


    [Fact]
    public void ConvertToString_CheckIfConversionIsCorrect()
    {
        Matrix utf8Message = new Matrix(new int[,]
        {
            {0,1,1,0,1,1,0,0,0,1,1,0,0,0,0,1,0,1,1,0,0,0,1,0,0,1,1,0,0,0,0,1,0,1,1,1,0,0,1,1}
        });

        string originalMessage = TextConverter.ConvertToOriginalFormat(utf8Message);
        
        Assert.True(originalMessage == "labas");


    }

    [Fact]
    public void CalculateSimilarity_CheckIfSimilarityCorrect()
    {
        string a = "Labas, kaip tau sekasi? 1234!@";
        string b = "Labas, kaip tau 101010? 1234!@";

        double similarity = 24.0 / 30.0 * 100;

        Assert.True(TextConverter.CalculateSimilarity(a, b) == similarity);
        
    }
    
    
}