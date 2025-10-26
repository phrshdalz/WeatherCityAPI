using WeatherCityAPI.GetWay;
using WeatherCityAPI.Interface;
using WeatherCityAPI.Interface.Geteway;
using WeatherCityAPI.Services;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(cfg => { }, typeof(Program));

// Register Weather Service
builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddScoped<IOpenWeather, OpenWeather>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
