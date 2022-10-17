using Grpc.Demo;
using Grpc.Demo.Client.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new OpenApiInfo { Title = "Wheather API", Version = "v1" });
    x.EnableAnnotations();
});

builder.Services.AddGrpc();
builder.Services.AddGrpcClient<WheatherService.WheatherServiceClient>(x =>
{
    x.Address = new Uri("http://localhost:7071");
});

builder.Services.AddScoped<IWeatherService, WeatherService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Wheather API v1");
        c.RoutePrefix = "api/swagger";
    });
}

app.MapGrpcService<WeatherService>();

app.MapControllers();

app.Run();
