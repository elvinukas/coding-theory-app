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

GroupElement.SetGroupSize(5);
GroupElement a = new GroupElement(0);
GroupElement b = new GroupElement(5);
GroupElement c = new GroupElement(2);
GroupElement d = new GroupElement(10);
GroupElement e = new GroupElement(3);

GroupElement sumResult = c + e;
GroupElement subResult = new GroupElement(10) - new GroupElement(2);
Console.WriteLine(sumResult.value);
Console.WriteLine(subResult.value);
Console.WriteLine((new GroupElement(1) - new GroupElement(2)).value);

