namespace app.Math;

public class Field
{
    public int q { get; private set; } = 2; // the size of the group, is static (global) for all group elements.
                                            // default value 2

    public Field(int size)
    {
        SetFieldSize(size);
    }
    
    public void SetFieldSize(int value)
    {
        if (value < 2 || !IsPrime(value))
        {
            throw new ArgumentException("The group size must be a prime number.");
        }

        q = value;
    }
    
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


    public static bool operator ==(Field a, Field b)
    {
        if (!(a != b))
        {
            return true;
        }

        return false;

    }


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