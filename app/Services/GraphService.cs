using System.Diagnostics.Eventing.Reader;
using app.Algorithms;
using app.Controllers;
using app.Math;
using app.Models;
using app.Models.Graph;
using ScottPlot;
using SixLabors.ImageSharp.Metadata.Profiles.Icc;

namespace app.Services;

public class GraphService : IGraphService
{
    public double Frequency { get; set; }

    public GraphService(double frequency)
    {
        Frequency = frequency;
    }

    public GraphResponse Paint(GraphRequest request)
    {
        List<List<double>> errorPercentages = GetData(request);

        

        double[] xValues = errorPercentages.Select(x => x[0]).ToArray();
        double[] linearGraph = errorPercentages.Select(x => x[1]).ToArray();
        double[] ogSimilarities = errorPercentages.Select(x => x[2]).ToArray();
        double[] encodedSimilarities = errorPercentages.Select(x => x[3]).ToArray();
        
        (double? crossValue, int? crossIndex) = FindWhereLinesFirstCross(encodedSimilarities, linearGraph);

        Plot plt = new Plot();
        
        if (crossIndex != null && crossValue != null)
        {
           var line = plt.Add.VerticalLine(xValues[(int)crossIndex]);
           line.Text = "Encoding becomes less effective after " + xValues[(int)crossIndex].ToString("F3");
           line.LineWidth = 5;
           line.LabelOppositeAxis = true;
        }

        //var a = plt.Add.SignalXY(xValues, ogSimilarities);
        var a = plt.Add.SignalXY(xValues, linearGraph);
        a.LineWidth = 3;
        a.LegendText = "Similarity with no encoding";
        var b = plt.Add.SignalXY(xValues,encodedSimilarities);
        b.LineWidth = 3;
        b.LegendText = "Similarity with encoding";
        
        plt.XLabel("Channel error percentage");
        plt.YLabel("Similarity % to original text");
        plt.Title("Encoding effectiveness graph");
        
        plt.Axes.SetLimitsX(0, 1);
        plt.Axes.SetLimitsY(0, 100);
        plt.ShowLegend(Alignment.UpperRight, Orientation.Horizontal);

        plt.SavePng(request.FilePath, 800, 600);
        

        return new GraphResponse
        {
            Message = "Graph encoded successfully.",
            FileName = "graph.png",
            CrossErrorPercentage = crossValue
        };

    }

    private List<List<double>> GetData(GraphRequest request)
    {
        Matrix gMatrix = new Matrix(MatrixConverter.ConvertToIntArray(request.GeneratorMatrix));
        Matrix ogMessage = new Matrix(MatrixConverter.ConvertToIntArray(request.OriginalMessage));
        int ogMessageLength = ogMessage.ToString().Replace(" ", "").Replace("\n", "").Length;
        LinearEncodingAlgorithm encodingAlgorithm =
            new LinearEncodingAlgorithm(ogMessage, gMatrix, gMatrix.Rows, gMatrix.Columns);
        Matrix encodedMessage = encodingAlgorithm.EncodedMessage;
        // true enables the message encoding by parts

        List<List<double>> errorPercentages = new List<List<double>>();
        RandomNumberGenerator rng = new RandomNumberGenerator();
        Channel channelOg, channelEncoded;
        StepByStepDecodingAlgorithm algorithm;
        

        // gathering all data for the graph
        for (double ep = 0; ep <= 1; ep += Frequency) // ep - error percentage
        {
            channelOg = new Channel(ogMessage, ep, rng);
            channelEncoded = new Channel(encodedMessage, ep, rng);
            
            // converting to string so that it is easier to already use pre-existing classes for similarity calculation
            string ogMessageErr = channelOg.ReceivedMessage.ToString().Replace(" ", "").Replace("\n", "");
            Matrix encodedMessageErr = channelEncoded.ReceivedMessage;
            algorithm = new StepByStepDecodingAlgorithm(gMatrix, ogMessageLength);
            Matrix decodedMessage = algorithm.DecodeMessage(encodedMessageErr);

            string ogMessageFormatted = ogMessage.ToString().Replace(" ", "").Replace("\n", "");
            
            double ogSimilarity = TextConverter
                .CalculateSimilarity(ogMessageErr, ogMessageFormatted);
            double encodedSimilarity = TextConverter
                .CalculateSimilarity(decodedMessage.ToString().Replace(" ", "").Replace("\n", ""), ogMessageFormatted);
            double linearGraph = 100 - ep * 100;
            
            List<double> percentages = new List<double>();
            percentages.Add(ep);
            percentages.Add(linearGraph);
            percentages.Add(ogSimilarity);
            percentages.Add(encodedSimilarity);
            errorPercentages.Add(percentages);

        }

        return errorPercentages;

    }

    private (double?, int?) FindWhereLinesFirstCross(double[] array1, double[] array2) {
        // index1 and index2 mean what type of info 
        // from list is being checked (for example lineargraph and encodedSimilarity) 
        for (int i = 0; i < array1.Length; i++)
        {
            if (array1[i] < array2[i])
            {
                return (array1[i], i);
            }
        }
        return (null, null);


    }
    
}