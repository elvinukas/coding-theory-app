namespace app.Math;

/// <summary>
/// This class is used to simulate a Field (the value q).
/// <example>While working with binary matrices, the value of field is always <c>q = 2</c></example>
/// </summary>
public class Field
{
    public int q { get; private set; } = 2; // the size of the group, is static (global) for all group elements.
                                            // default value 2

    /// <summary>
    /// Standard Field constructor.
    /// </summary>
    /// <param name="size">Specifying the field size.</param>
    public Field(int size)
    {
        SetFieldSize(size);
    }
    
    /// <summary>
    /// Method to set the field size.
    /// </summary>
    /// <param name="value">Value of the field. Must be a prime number.</param>
    /// <exception cref="ArgumentException">Throws if the value is not a prime number.</exception>
    public void SetFieldSize(int value)
    {
        if (value < 2 || !IsPrime(value))
        {
            throw new ArgumentException("The group size must be a prime number.");
        }

        q = value;
    }
    
    /// <summary>
    /// Checking if number is prime.
    /// </summary>
    /// <param name="number">Checked number.</param>
    /// <returns><c>bool</c></returns>
    private static bool IsPrime(int number)
    {
        for (int i = 2; i < number / 2; ++i)
        {
            if (number % i == 0)
            {
                return false;
            }
        }

        return true;


    }

    /// <summary>
    /// Equality operator between two fields.
    /// </summary>
    /// <param name="a"><c>Field</c> object.</param>
    /// <param name="b"><c>Field</c> object.</param>
    /// <returns><c>bool</c></returns>
    public static bool operator ==(Field a, Field b)
    {
        if (!(a != b))
        {
            return true;
        }

        return false;

    }

    /// <summary>
    /// Inequality operator between two fields.
    /// </summary>
    /// <param name="a"><c>Field</c> object.</param>
    /// <param name="b"><c>Field</c> object.</param>
    /// <returns><c>bool</c></returns>
    public static bool operator !=(Field a, Field b)
    {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
        {
            return false;
        }
        
        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
        {
            return true;
        }
        
        if (a.q != b.q)
        {
            return true;
        }

        return false;


    }
    
    
    
}