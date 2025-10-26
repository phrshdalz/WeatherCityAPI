using System.Text.Json.Serialization;

namespace WeatherCityAPI.Models;

public class GeocodingDTO
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("lat")]
    public double Lat { get; set; }
    
    [JsonPropertyName("lon")]
    public double Lon { get; set; }
    
    [JsonPropertyName("country")]
    public string Country { get; set; } = string.Empty;
    
    [JsonPropertyName("state")]
    public string State { get; set; } = string.Empty;
}

public class OpenWeatherWeatherDTO
{
    [JsonPropertyName("coord")]
    public Coord Coord { get; set; } = new();
    
    [JsonPropertyName("weather")]
    public List<Weather> Weather { get; set; } = new();
    
    [JsonPropertyName("main")]
    public Main Main { get; set; } = new();
    
    [JsonPropertyName("wind")]
    public Wind Wind { get; set; } = new();
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

public class Coord
{
    [JsonPropertyName("lat")]
    public double Lat { get; set; }
    
    [JsonPropertyName("lon")]
    public double Lon { get; set; }
}

public class Weather
{
    [JsonPropertyName("main")]
    public string Main { get; set; } = string.Empty;
    
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
}

public class Main
{
    [JsonPropertyName("temp")]
    public double Temp { get; set; }
    
    [JsonPropertyName("humidity")]
    public double Humidity { get; set; }
}

public class Wind
{
    [JsonPropertyName("speed")]
    public double Speed { get; set; }
}

public class OpenWeatherPollutionDTO
{
    [JsonPropertyName("list")]
    public List<PollutionData> List { get; set; } = new();
}

public class PollutionData
{
    [JsonPropertyName("main")]
    public MainData Main { get; set; } = new();
    
    [JsonPropertyName("components")]
    public Components Components { get; set; } = new();
    
    [JsonPropertyName("dt")]
    public int Dt { get; set; }
}

public class MainData
{
    [JsonPropertyName("aqi")]
    public int Aqi { get; set; }
}

public class Components
{
    [JsonPropertyName("co")]
    public double? Co { get; set; }

    [JsonPropertyName("no")]
    public double? No { get; set; }

    [JsonPropertyName("no2")]
    public double? No2 { get; set; }

    [JsonPropertyName("o3")]
    public double? O3 { get; set; }

    [JsonPropertyName("so2")]
    public double? So2 { get; set; }

    [JsonPropertyName("pm2_5")]
    public double? Pm2_5 { get; set; }

    [JsonPropertyName("pm10")]
    public double? Pm10 { get; set; }

    [JsonPropertyName("nh3")]
    public double? Nh3 { get; set; }
}
public class ServiceResultDTO
{
    public int cod { get; set; }
    public string message { get; set; }

}

