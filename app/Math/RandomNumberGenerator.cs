namespace app.Math;


/// <summary>
/// This class implements the <c>INumGen</c> interface to generate random numbers.
/// </summary>
public class RandomNumberGenerator : INumGen
{
    private double RandomNumber;
    private Random random;

    /// <summary>
    /// Constructor which generates a random number stored in <see cref="random"/>
    /// </summary>
    public RandomNumberGenerator()
    {
        this.random = new Random();
    }
    
    /// <summary>
    /// Method to generate a number.
    /// <remarks>This method is virtual, so that later on in the mock tests it is able to be overriden.</remarks>
    /// </summary>
    /// <returns></returns>
    public virtual double GenerateNumber()
    {
        return random.NextDouble();
    }
    
}