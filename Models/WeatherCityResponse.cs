namespace WeatherCityAPI.Models;

public class WeatherCityResponse
{
    public string City { get; set; } = string.Empty;
    public string TemperatureCelsius { get; set; }
    public string Humidity { get; set; }
    public string WindSpeed { get; set; }
    public int AirQualityIndex { get; set; }
    public List<Pollutant> MajorPollutants { get; set; } = new();
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public class Pollutant
{
    public string Name { get; set; } = string.Empty;
    public double Concentration { get; set; }
    public string Unit { get; set; } = string.Empty;
}

