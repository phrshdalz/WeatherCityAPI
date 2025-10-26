namespace WeatherCityAPI.Models;

public class ApiConfiguration
{
    public string OpenWeatherMapApiKey { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = "https://api.openweathermap.org";
}

