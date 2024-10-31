using app.Math;

namespace app.Algorithms;

public interface IEncoding
{
    public static abstract Matrix Encode(Matrix originalMessage, Matrix gMatrix);
    public static abstract void EncodeFile(string filePath, string encodedFilePath);
    
}