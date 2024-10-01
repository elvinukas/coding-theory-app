namespace tests;
using app.Math;

public class StandardArrayGeneratorUnitTests
{

    [Fact]
    public void Constructor_CheckIfArgumentIsCorrect()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            StandardArrayGenerator standardArrayGenerator = new StandardArrayGenerator(null);
        });
    }

    [Fact]
    public void Codewords_CheckIfAllPossibilitiesAreGenerated()
    {
        int[,] generatorMatrixMembers = { { 1, 0, 0, 0, 1 }, { 0, 1, 0, 0, 1 }, { 0, 0, 1, 1, 1 } };
        Matrix generatorMatrix = new Matrix(generatorMatrixMembers);
        StandardArrayGenerator standardArrayGenerator = new StandardArrayGenerator(generatorMatrix);
        
        // 2^3 = 8
        // {0, 0, 0} {0, 0, 1}
        // {0, 1, 0} {0, 1, 1}
        // {1, 0, 0} {1, 0, 1}
        // {1, 1, 0} {1, 1, 1}
        
        // they should generate these valid codewords
        // |
        // |
        // V
    
        List<Matrix> possibleCodewords = new List<Matrix>();
        possibleCodewords = standardArrayGenerator.Codewords;
    
        List<Matrix> expectedCodewords = new List<Matrix>
        {
            new Matrix(new int[,] { {0, 0, 0, 0, 0 }}),
            new Matrix(new int[,] { {0, 0, 1, 1, 1 }}),
            new Matrix(new int[,] { {0, 1, 0, 0, 1 }}),
            new Matrix(new int[,] { {0, 1, 1, 1, 0 }}),
            new Matrix(new int[,] { {1, 0, 0, 0, 1 }}),
            
            new Matrix(new int[,] { {1, 0, 1, 1, 0 }}),
            new Matrix(new int[,] { {1, 1, 0, 0, 0 }}),
            new Matrix(new int[,] { {1, 1, 1, 1, 1 }})
        };
    
        foreach (var expectedCodeword in expectedCodewords)
        {
            Assert.Contains(possibleCodewords, codeword => codeword.ToString() == expectedCodeword.ToString());
        }
        
    
    }
    
    // need test for checking the standard array table too, but later
    
    
    
}