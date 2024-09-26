using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using app.Math;


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

//GroupElement.SetGroupSize(5);
Field field = new Field(2);
FieldElement a = new FieldElement(0, field);
FieldElement b = new FieldElement(5, field);
FieldElement c = new FieldElement(2, field);
FieldElement d = new FieldElement(10, field);
FieldElement e = new FieldElement(3, field);

FieldElement sumResult = c + e;
FieldElement subResult = new FieldElement(10, field) - new FieldElement(2, field);
Console.WriteLine(sumResult.value);
Console.WriteLine(subResult.value);
Console.WriteLine((new FieldElement(1, field) - new FieldElement(2, field)).value);

Console.WriteLine("Multiply result:");
FieldElement vienetas = new FieldElement(1, field);
FieldElement antrasVienetas = new FieldElement(1, field);
Console.WriteLine((vienetas * antrasVienetas).value);

Console.WriteLine();
// int[,] elements = { { 1, 2 }, {3, 4} };
// Matrix matrix = new Matrix(elements);
int[,] moreElements = { { 1, 2, 3, 884, 9095 }, { 8, 7, 8, 9, 10 } };
Matrix biggerMatrix = new Matrix(moreElements);

//Console.WriteLine(matrix.ToString());
Console.Write(biggerMatrix.ToString());

int[,] newElements = { { 1, 2, 3, 4, 5}, { 3, 4, 5, 6, 6 } };
Matrix newMatrix = new Matrix(newElements);
Console.Write(newMatrix.ToString());

