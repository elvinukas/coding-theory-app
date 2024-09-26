using System;
using System.Text.RegularExpressions;

namespace app.Math;

public class GroupElement
{
    public static int q { get; private set; } = 2; // the size of the group, is static (global) for all group elements.
                                                   // default value 2
    public int value { get; private set; } // the value of a group element

    public static void SetGroupSize(int value)
    {
        if (value < 2 || !IsPrime(value))
        {
            throw new ArgumentException("The group size must be a prime number.");
        }

        q = value;
    }
    
    // checking if the group element is correct, and if not fixing it according to the group size
    // [0, 1, 2] -> 6 / 3 = 2, tada 2 + 3 = 5 % 3 = 2
    public GroupElement(int value)
    {
        this.value = ((value % q) + q) % q;
    }

    
    // overloading + (addition) operator
    public static GroupElement operator +(GroupElement firstElement, GroupElement secondElement)
    {
        return new GroupElement((firstElement.value + secondElement.value) % q);
    }

    // overloading * (multiplication) operator
    public static GroupElement operator *(GroupElement firstElement, GroupElement secondElement)
    {
        return new GroupElement((firstElement.value * secondElement.value) % q);
    }
    
    
    // overloading - (subtraction) operator
    public static GroupElement operator -(GroupElement firstElement, GroupElement secondElement)
    {
        // division by modulo is not required, since the element value cannot exceed the group size bounds already
        // if this was any other number, then modulo division would be required
        return new GroupElement(firstElement.value - secondElement.value + q);
    }

    // i'm not sure whether division is required, so we will stick with these operators
    
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
    
}