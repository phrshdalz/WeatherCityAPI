using Microsoft.AspNetCore.Mvc;
using WeatherCityAPI.Interface;
using WeatherCityAPI.Services;

namespace WeatherCityAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;
    private readonly ILogger<WeatherController> _logger;

    public WeatherController(IWeatherService weatherService, ILogger<WeatherController> logger)
    {
        _weatherService = weatherService;
        _logger = logger;
    }

    [HttpGet("{cityName}")]
    [ProducesResponseType(typeof(WeatherCityAPI.Models.WeatherCityResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<WeatherCityAPI.Models.WeatherCityResponse>> GetCityWeather(string cityName)
    {
        if (string.IsNullOrWhiteSpace(cityName))
        {
            return BadRequest("City name cannot be empty");
        }

        try
        {
            var result = await _weatherService.GetCityWeatherAsync(cityName);
            
            if (result == null)
            {
                _logger.LogWarning("Weather data not found for city: {CityName}", cityName);
                return NotFound($"Weather data not found for city: {cityName}");
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving weather data for city: {CityName}", cityName);
            return StatusCode(500, "An error occurred while retrieving weather data");
        }
    }
}

