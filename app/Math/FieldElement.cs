using System;
using System.Text.RegularExpressions;

namespace app.Math;

/// <summary>
/// This class represents the actual field element (certain value with field).
/// <example>Can be used to compute operations with elements from non-standard fields, such as <c>base 5</c></example>
/// </summary>
public struct FieldElement
{
    public Field field;
    public int Value { get; internal set; } // the value of a group element
    
    
    // checking if the group element is correct, and if not fixing it according to the group size
    // [0, 1, 2] -> 6 / 3 = 2, tada 2 + 3 = 5 % 3 = 2
    
    /// <summary>
    /// Constructor for FieldElement.
    /// <para>Checks if the group element is correct and if not fixes it according to the group size.</para>
    /// </summary>
    /// <param name="value">The value that will be turned into a field element by checking with the linked field.</param>
    /// <param name="field">Field that will define <see cref="Value"/></param>
    public FieldElement(int value, Field field)
    {
        this.field = field;
        if (field.q == 2)
        {
            this.Value = value % 2;
        }
        else
        {
            this.Value = ((value % field.q) + field.q) % field.q;
        }
        
    }

    /// <summary>
    /// Equality operator for <c>FieldElement</c> objects.
    /// </summary>
    /// <param name="firstElement"><c>FieldElement</c> object</param>
    /// <param name="secondElement"><c>FieldElement</c> object</param>
    /// <returns></returns>
    public static bool operator ==(FieldElement firstElement, FieldElement secondElement)
    {
        if (!(firstElement != secondElement))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Inequality operator for <c>FieldElement</c> objects.
    /// </summary>
    /// <param name="firstElement"><c>FieldElement</c> object</param>
    /// <param name="secondElement"><c>FieldElement</c> object</param>
    /// <returns></returns>
    public static bool operator !=(FieldElement firstElement, FieldElement secondElement)
    {
        if (ReferenceEquals(firstElement, null) && ReferenceEquals(firstElement, null))
        {
            return false;
        }
        
        if (ReferenceEquals(firstElement, null) || ReferenceEquals(firstElement, null))
        {
            return true;
        }
        
        if (firstElement.field.q != secondElement.field.q)
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
    /// <summary>
    /// Addition operator for <c>FieldElement</c> objects.
    /// </summary>
    /// <param name="firstElement"><c>FieldElement</c> object</param>
    /// <param name="secondElement"><c>FieldElement</c> object</param>
    /// <returns></returns>
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
    /// <summary>
    /// Multiplication operator for <c>FieldElement</c> objects.
    /// </summary>
    /// <param name="firstElement"><c>FieldElement</c> object</param>
    /// <param name="secondElement"><c>FieldElement</c> object</param>
    /// <returns></returns>
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
    /// <summary>
    /// Subtraction operator for <c>FieldElement</c> objects.
    /// </summary>
    /// <param name="firstElement"><c>FieldElement</c> object</param>
    /// <param name="secondElement"><c>FieldElement</c> object</param>
    /// <returns></returns>
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