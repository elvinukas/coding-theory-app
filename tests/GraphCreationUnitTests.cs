using app.Algorithms;
using app.Controllers;
using app.Math;
using app.Models.Graph;
using app.Services;

namespace tests;

public class GraphCreationUnitTests
{

    [Fact]
    public void Paint_CheckIfGraphPaintingSucceeds()
    {
        string message = "labas rytas suraitytas, cia mes isbandome gera testa! Pabandome dar ilgesni teksta del geresniu rezultatu!";

        Matrix ogMessage = TextConverter.ConvertToBinaryMatrix(message);
        
        Matrix genMatrix = new Matrix(new int[,]
        {
            {1, 0, 0, 0, 1, 1, 0}, 
            {0, 1, 0, 0, 1, 0, 1},
            {0, 0, 1, 0, 1, 1, 1},
            {0, 0, 0, 1, 0, 1, 1}
        });
        
        string filePath = "../../../test-images/graph.png";

        GraphRequest request = new GraphRequest
        {
            OriginalMessage = MatrixConverter.ConvertTo2DList(ogMessage),
            GeneratorMatrix = MatrixConverter.ConvertTo2DList(genMatrix),
            FilePath = filePath
        };

        GraphService service = new GraphService(0.01);
        service.Paint(request);

    }
    
    [Fact]
    public void Paint_CheckIfGraphPaintingSucceedsWithImage()
    {
        string message = "labas rytas suraitytas, cia mes isbandome gera testa! Pabandome dar ilgesni teksta del geresniu rezultatu!";
        Matrix ogMessage = TextConverter.ConvertToBinaryMatrix(message);
        
        Matrix genMatrix = new Matrix(new int[,]
        {
            {1, 0, 0, 0, 1, 1, 0}, 
            {0, 1, 0, 0, 1, 0, 1},
            {0, 0, 1, 0, 1, 1, 1},
            {0, 0, 0, 1, 0, 1, 1}
        });
        
        string filePath = "../../../test-images/graph.png";

        GraphRequest request = new GraphRequest
        {
            OriginalMessage = MatrixConverter.ConvertTo2DList(ogMessage),
            GeneratorMatrix = MatrixConverter.ConvertTo2DList(genMatrix),
            FilePath = filePath
        };

        GraphService service = new GraphService(0.001);
        service.Paint(request);

    }
    
    
    
    
}