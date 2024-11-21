using app.Math;
using app.Services;




var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddTransient<IEncodingService, VectorEncodingService>();
builder.Services.AddTransient<IEncodingService, TextEncodingService>();
//builder.Services.AddTransient<IEncodingService, ImageEncodingService>();

builder.Services.AddTransient<IDecodingService, VectorDecodingService>();

builder.Services.AddTransient<IChannelService, VectorChannelService>();
builder.Services.AddTransient<IMatrixGen, GeneratorMatrixGenerator>();
builder.Services.AddTransient<INumGen, RandomNumberGenerator>();
builder.Services.AddTransient<IGenerator, MatrixGenService>();

builder.Services.AddSingleton<EncodingServiceFactory>();
builder.Services.AddSingleton<ChannelServiceFactory>();
builder.Services.AddSingleton<DecodingServiceFactory>();
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




