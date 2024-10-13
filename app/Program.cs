using System.Text.RegularExpressions;
using app.Algorithms;
using Microsoft.VisualBasic;
using app.Math;
using SixLabors.ImageSharp;


/*
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();

*/

/// ------------------------------------------------------
/// ------------------------------------------------------
/// ------------------------------------------------------

// //GroupElement.SetGroupSize(5);
// Field field = new Field(2);
// FieldElement a = new FieldElement(0, field);
// FieldElement b = new FieldElement(5, field);
// FieldElement c = new FieldElement(2, field);
// FieldElement d = new FieldElement(10, field);
// FieldElement e = new FieldElement(3, field);
//
// FieldElement sumResult = c + e;
// FieldElement subResult = new FieldElement(10, field) - new FieldElement(2, field);
// Console.WriteLine(sumResult.Value);
// Console.WriteLine(subResult.Value);
// Console.WriteLine((new FieldElement(1, field) - new FieldElement(2, field)).Value);
//
// Console.WriteLine("Multiply result:");
// FieldElement vienetas = new FieldElement(1, field);
// FieldElement antrasVienetas = new FieldElement(1, field);
// Console.WriteLine((vienetas * antrasVienetas).Value);
//
// Console.WriteLine();
// // int[,] elements = { { 1, 2 }, {3, 4} };
// // Matrix matrix = new Matrix(elements);
// int[,] moreElements = { { 1, 2, 3, 884, 9095 }, { 8, 7, 8, 9, 10 } };
// Matrix biggerMatrix = new Matrix(moreElements);
//
// //Console.WriteLine(matrix.ToString());
// //Console.Write(biggerMatrix.ToString());
//
// int[,] newElements = { { 1, 2, 3, 4, 5}, { 3, 4, 5, 6, 6 } };
// Matrix newMatrix = new Matrix(newElements);
// //Console.Write(newMatrix.ToString());
//
//
// int[,] example = { { 1, 1, 0 } };
// int[,] generuojantysElementai = { { 1, 1, 0, 0 }, { 0, 1, 1, 1 }, { 1, 0, 1, 0 } };
// Matrix generuojantiMatrica = new Matrix(generuojantysElementai);
// Matrix zinute = new Matrix(example);
//
// Console.Write((zinute * generuojantiMatrica).ToString());
//
// int[,] exampleGenMatricos = { { 1, 0, 0, 0, 1 }, { 0, 1, 0, 0, 1 }, { 0, 0, 1, 1, 1 } };
// Matrix genMatrica = new Matrix(exampleGenMatricos);
//
// StandardArrayGenerator standardArrayGenerator = new StandardArrayGenerator(genMatrica);
//
//
// string message = "labas";
// Matrix userMessage = TextConverter.ConvertToBinaryMatrix(message);

string binarySavePath = "../tests/test-images/test.bin";
string decodedBinarySavePath = "../tests/test-images/test_decoded.bin";
Image image = Image.Load("../tests/test-images/test.jpeg");
Matrix convertedMatrix = ImageConverter.ConvertToBinaryMatrix(image, binarySavePath);

Matrix generatorMatrix = new Matrix(new int[,]
{
    { 1, 0, 0, 0, 1, 1, 0 },
    { 0, 1, 0, 0, 1, 0, 1 },
    { 0, 0, 1, 0, 1, 1, 1 },
    { 0, 0, 0, 1, 0, 1, 1 }
});

LinearEncodingAlgorithm encodingAlgorithm = new LinearEncodingAlgorithm(convertedMatrix, generatorMatrix, 4, 7);

Matrix encodedImage = encodingAlgorithm.EncodedMessage;
Channel channel = new Channel(encodedImage, 0.01);
Matrix withErrors = channel.ReceivedMessage;
Matrix decodedImage =
    StepByStepDecodingAlgorithm.Decode(generatorMatrix, withErrors, encodingAlgorithm.OriginalMessageLength);

ImageConverter.ConvertToOriginalFormat(decodedImage, "../../tests/test-images/test_decoded.bmp");





