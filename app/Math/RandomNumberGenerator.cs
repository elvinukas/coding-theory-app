namespace app.Math;

public class RandomNumberGenerator
{
    private double RandomNumber;
    private Random random;

    public RandomNumberGenerator()
    {
        this.random = new Random();
    }

    // this method is virtual, so that later on in the mock tests it is able to be overriden
    public virtual double GetNewRandomNumber()
    {
        return random.NextDouble();
    }
    
}