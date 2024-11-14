using System.Text.RegularExpressions;
using app.Algorithms;
using Microsoft.VisualBasic;
using app.Math;
using app.Services;
using SixLabors.ImageSharp;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddTransient<IEncodingService, VectorEncodingService>();
//builder.Services.AddTransient<IEncodingService, TextEncodingService>();
//builder.Services.AddTransient<IEncodingService, VectorEncodingService>();

builder.Services.AddSingleton<EncodingServiceFactory>();
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();

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
app.MapControllers();



app.Run();




