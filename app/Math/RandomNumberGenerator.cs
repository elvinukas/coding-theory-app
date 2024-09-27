namespace app.Math;

public class RandomNumberGenerator
{
    public double RandomNumber { get; private set;}
    private Random random;

    public RandomNumberGenerator()
    {
        this.random = new Random();
        this.RandomNumber = GetNewRandomNumber();
    }

    public double GetNewRandomNumber()
    {
        this.RandomNumber = random.NextDouble();
        return RandomNumber;
    }
    
}