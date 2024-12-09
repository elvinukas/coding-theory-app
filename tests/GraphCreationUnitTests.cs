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
        Matrix ogMessage = new Matrix(new int[,]
        {
            {1, 0, 0, 1}
        });
        
        Matrix genMatrix = new Matrix(new int[,]
        {
            {1, 0, 0, 0, 1, 1, 0}, 
            {0, 1, 0, 0, 1, 0, 1},
            {0, 0, 1, 0, 1, 1, 1},
            {0, 0, 0, 1, 0, 1, 1}
        });

        GraphRequest request = new GraphRequest
        {
            OriginalMessage = MatrixConverter.ConvertTo2DList(ogMessage),
            GeneratorMatrix = MatrixConverter.ConvertTo2DList(genMatrix)
        };

        GraphService service = new GraphService(0.01);
        service.Paint(request);

    }
    
    [Fact]
    public void Paint_CheckIfGraphPaintingSucceedsWithImage()
    {
        string message = "labas rytas suraitytas, cia mes isbandome gera testa! Pabandome dar ilgesni teksta del geresniu rezultatu!" +
                         "Galbut net dar ilgesni, kad butu geresni duomenys! Kuo ilgesnis tekstas, tuo geriau atsispindes rezultatai! Geras! 123";
        Matrix ogMessage = TextConverter.ConvertToBinaryMatrix(message);
        
        Matrix genMatrix = new Matrix(new int[,]
        {
            {1, 0, 0, 0, 1, 1, 0}, 
            {0, 1, 0, 0, 1, 0, 1},
            {0, 0, 1, 0, 1, 1, 1},
            {0, 0, 0, 1, 0, 1, 1}
        });
        

        GraphRequest request = new GraphRequest
        {
            OriginalMessage = MatrixConverter.ConvertTo2DList(ogMessage),
            GeneratorMatrix = MatrixConverter.ConvertTo2DList(genMatrix)
        };

        GraphService service = new GraphService(0.01);
        service.Paint(request);

    }
    
    
    
    
}