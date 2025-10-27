using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using WeatherCityAPI.Controllers;
using WeatherCityAPI.Interface;
using WeatherCityAPI.Models;
using Xunit;

namespace WeatherCity.Test;

public class WeatherControllerTests
{
    private readonly Mock<IWeatherService> _weatherServiceMock;
    private readonly Mock<ILogger<WeatherController>> _loggerMock;
    private readonly WeatherController _controller;

    public WeatherControllerTests()
    {
        _weatherServiceMock = new Mock<IWeatherService>();
        _loggerMock = new Mock<ILogger<WeatherController>>();
        _controller = new WeatherController(_weatherServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetCityWeather_WithValidCityName_ReturnsOkResult()
    {
        // Arrange
        var cityName = "London";
        var mockWeatherResponse = new WeatherCityResponse
        {
            City = "London",
            TemperatureCelsius = "15 C",
            Humidity = "65 %",
            WindSpeed = "10 meter/sec",
            AirQualityIndex = 2,
            Latitude = 51.5074,
            Longitude = -0.1278,
            MajorPollutants = new List<Pollutant>
            {
                new Pollutant { Name = "PM2.5", Concentration = 15.5, Unit = "μg/m³" }
            }
        };

        _weatherServiceMock.Setup(service => service.GetCityWeatherAsync(cityName))
            .ReturnsAsync(mockWeatherResponse);

        // Act
        var result = await _controller.GetCityWeather(cityName);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<WeatherCityResponse>(okResult.Value);
        Assert.Equal(cityName, returnValue.City);
        Assert.Equal("London", returnValue.City);
        Assert.Equal("15 C", returnValue.TemperatureCelsius);
        Assert.Equal(2, returnValue.AirQualityIndex);
    }

    [Fact]
    public async Task GetCityWeather_WithEmptyCityName_ReturnsBadRequest()
    {
        // Arrange
        var cityName = "";

        // Act
        var result = await _controller.GetCityWeather(cityName);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("City name cannot be empty", badRequestResult.Value);
    }

    [Fact]
    public async Task GetCityWeather_WithWhitespaceCityName_ReturnsBadRequest()
    {
        // Arrange
        var cityName = "   ";

        // Act
        var result = await _controller.GetCityWeather(cityName);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("City name cannot be empty", badRequestResult.Value);
    }

    [Fact]
    public async Task GetCityWeather_WithNullCityName_ReturnsBadRequest()
    {
        // Arrange
        string cityName = null;

        // Act
        var result = await _controller.GetCityWeather(cityName);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("City name cannot be empty", badRequestResult.Value);
    }

    [Fact]
    public async Task GetCityWeather_WhenServiceReturnsNull_ReturnsNotFound()
    {
        // Arrange
        var cityName = "NonExistentCity";

        _weatherServiceMock.Setup(service => service.GetCityWeatherAsync(cityName))
            .ReturnsAsync((WeatherCityResponse)null);

        // Act
        var result = await _controller.GetCityWeather(cityName);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal($"Weather data not found for city: {cityName}", notFoundResult.Value);
    }

    [Fact]
    public async Task GetCityWeather_CallsServiceWithCorrectCityName()
    {
        // Arrange
        var cityName = "Tehran";
        _weatherServiceMock.Setup(service => service.GetCityWeatherAsync(cityName))
            .ReturnsAsync(new WeatherCityResponse { City = cityName });

        // Act
        await _controller.GetCityWeather(cityName);

        // Assert
        _weatherServiceMock.Verify(service => service.GetCityWeatherAsync(cityName), Times.Once);
    }

    [Fact]
    public async Task GetCityWeather_WhenServiceThrowsException_ExceptionPropagates()
    {
        // Arrange
        var cityName = "London";
        _weatherServiceMock.Setup(service => service.GetCityWeatherAsync(cityName))
            .ThrowsAsync(new Exception("Error 401 - Unauthorized. Invalid API key."));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(async () =>
            await _controller.GetCityWeather(cityName));

        
    }

    [Theory]
    [InlineData("New York")]
    [InlineData("Tokyo")]
    [InlineData("Paris")]
    [InlineData("Berlin")]
    public async Task GetCityWeather_WithValidCities_ReturnsSuccess(string cityName)
    {
        // Arrange
        _weatherServiceMock.Setup(service => service.GetCityWeatherAsync(cityName))
            .ReturnsAsync(new WeatherCityResponse
            {
                City = cityName,
                TemperatureCelsius = "20 C",
                Humidity = "50 %",
                WindSpeed = "5 meter/sec",
                AirQualityIndex = 1,
                Latitude = 40.7128,
                Longitude = -74.0060
            });

        // Act
        var result = await _controller.GetCityWeather(cityName);

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);
    }
}


