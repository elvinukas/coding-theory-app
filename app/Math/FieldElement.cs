using System;
using System.Text.RegularExpressions;

namespace app.Math;

public class FieldElement
{
    public Field field;
    public int Value { get; private set; } // the value of a group element
    
    
    // checking if the group element is correct, and if not fixing it according to the group size
    // [0, 1, 2] -> 6 / 3 = 2, tada 2 + 3 = 5 % 3 = 2
    public FieldElement(int value, Field field)
    {
        this.field = field;
        this.Value = ((value % field.q) + field.q) % field.q;
    }


    public static bool operator ==(FieldElement firstElement, FieldElement secondElement)
    {
        if (!(firstElement != secondElement))
        {
            return true;
        }

        return false;
    }

    public static bool operator !=(FieldElement firstElement, FieldElement secondElement)
    {
        if (firstElement.field != secondElement.field)
        {
            return true;
        }

        if (firstElement.Value != secondElement.Value)
        {
            return true;
        }

        return false;

    }

    
    // overloading + (addition) operator
    public static FieldElement operator +(FieldElement firstElement, FieldElement secondElement)
    {
        if (firstElement.field.q != secondElement.field.q)
        {
            throw new InvalidOperationException("Field elements must have the same field size q.");
        }
        
        int result = (firstElement.Value + secondElement.Value) % firstElement.field.q;
        return new FieldElement(result, firstElement.field);
    }

    // overloading * (multiplication) operator
    public static FieldElement operator *(FieldElement firstElement, FieldElement secondElement)
    {
        if (firstElement.field.q != secondElement.field.q)
        {
            throw new InvalidOperationException("Field elements must have the same field size q.");
        }

        int result = (firstElement.Value * secondElement.Value) % firstElement.field.q;
        return new FieldElement(result, firstElement.field);
    }
    
    
    // overloading - (subtraction) operator
    public static FieldElement operator -(FieldElement firstElement, FieldElement secondElement)
    {
        if (firstElement.field.q != secondElement.field.q)
        {
            throw new InvalidOperationException("Field elements must have the same field size q.");
        }
        
        // division by modulo is not required, since the element value cannot exceed the group size bounds already
        // if this was any other number, then modulo division would be required
        return new FieldElement((firstElement.Value - secondElement.Value + firstElement.field.q), firstElement.field);
    }

    // i'm not sure whether division is required, so we will stick with these operators
    
  
    
}