namespace app.Algorithms;
using app.Math;

/// <summary>
/// The step-by-step decoding algorithm depends on matrix transposition, parity matrix and the generator matrix structure.
/// <br></br>
/// Each generator matrix has an identity matrix within it and some aditional bits for adapting the message to the desired length.
/// Now, the parity matrix H (the matrix which is used to check the encoded codeword) is generated like this:
/// G * H^T = 0, where T is transposition applied to the matrix.
/// Now H is constructed by using 
/// </summary>


public static class StepByStepDecodingAlgorithm
{
    
}