using WeatherCityAPI.Models;

namespace WeatherCityAPI.Interface;

public interface IWeatherService
{
    Task<WeatherCityResponse?> GetCityWeatherAsync(string cityName);
}

