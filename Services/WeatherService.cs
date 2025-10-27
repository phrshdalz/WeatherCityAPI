using AutoMapper;
using RestSharp;
using System.Text.Json;
using WeatherCityAPI.GetWay;
using WeatherCityAPI.Interface;
using WeatherCityAPI.Interface.Geteway;
using WeatherCityAPI.Models;

namespace WeatherCityAPI.Services;

public class WeatherService : IWeatherService
{
    private readonly IOpenWeather _openWeather;
    private readonly IMapper _mapper;

    public WeatherService(IOpenWeather openWeather, IMapper mapper)
    {
        _openWeather = openWeather;
        _mapper = mapper;
    }

    public async Task<WeatherCityResponse?> GetCityWeatherAsync(string cityName/*,string Units*/)
    {
        WeatherCityResponse weatherCityResponse = new();

        var geocodingData = await _openWeather.GetGeocodingDataAsync(cityName);
        if (geocodingData == null || geocodingData.Count == 0)
        {
            return null;
        }
        var location = geocodingData.First();

        var weatherData = await _openWeather.GetWeatherDataByCoordinatesAsync(location.Lat, location.Lon);
        if (weatherData == null)
        {
            return null;
        }

        var pollutionData = await _openWeather.GetAirPollutionDataAsync(location.Lat, location.Lon);

        weatherCityResponse=CustomMapper(geocodingData, weatherData, pollutionData/*, Units*/);

        return weatherCityResponse;
    }


    private WeatherCityResponse CustomMapper(List<GeocodingDTO> geocodingDTO, OpenWeatherWeatherDTO openWeatherWeatherDTO, OpenWeatherPollutionDTO? openWeatherPollutionDTO, string Units = "standard")
    {
        WeatherCityResponse weatherCityDTO = new();
        var location = geocodingDTO.First();
        return Units switch
        {
            "standard" => new WeatherCityResponse
            {
                City = location.Name,
                TemperatureCelsius = openWeatherWeatherDTO.Main.Temp + " C", 
                Humidity = openWeatherWeatherDTO.Main.Humidity + " %",
                WindSpeed = openWeatherWeatherDTO.Wind.Speed + " meter/sec",
                AirQualityIndex = openWeatherPollutionDTO?.List.FirstOrDefault()?.Main.Aqi ?? 0,
                MajorPollutants = ExtractPollutants(openWeatherPollutionDTO),
                Latitude = location.Lat,
                Longitude = location.Lon
            },
            //"metric"=> new WeatherCityResponse
            //{
            //    City = location.Name,
            //    TemperatureCelsius = openWeatherWeatherDTO.Main.Temp + " C", 
            //    Humidity = openWeatherWeatherDTO.Main.Humidity + " %",
            //    WindSpeed = openWeatherWeatherDTO.Wind.Speed + " meter/sec",
            //    AirQualityIndex = openWeatherPollutionDTO?.List.FirstOrDefault()?.Main.Aqi ?? 0,
            //    MajorPollutants = ExtractPollutants(openWeatherPollutionDTO),
            //    Latitude = location.Lat,
            //    Longitude = location.Lon
            //},
            //"imperial" => new WeatherCityResponse
            //{
            //    City = location.Name,
            //    TemperatureCelsius = openWeatherWeatherDTO.Main.Temp + " F", 
            //    Humidity = openWeatherWeatherDTO.Main.Humidity + " %",
            //    WindSpeed = openWeatherWeatherDTO.Wind.Speed + " miles/hour",
            //    AirQualityIndex = openWeatherPollutionDTO?.List.FirstOrDefault()?.Main.Aqi ?? 0,
            //    MajorPollutants = ExtractPollutants(openWeatherPollutionDTO),
            //    Latitude = location.Lat,
            //    Longitude = location.Lon
            //},
        };
    }



    private List<Pollutant> ExtractPollutants(OpenWeatherPollutionDTO? pollutionData)
    {
        var pollutants = new List<Pollutant>();

        if (pollutionData?.List.FirstOrDefault()?.Components == null)
        {
            return pollutants;
        }

        var components = pollutionData.List.First().Components;

        if (components.Co.HasValue)
            pollutants.Add(new Pollutant { Name = "CO", Concentration = components.Co.Value, Unit = "μg/m³" });

        if (components.No.HasValue)
            pollutants.Add(new Pollutant { Name = "NO", Concentration = components.No.Value, Unit = "μg/m³" });

        if (components.No2.HasValue)
            pollutants.Add(new Pollutant { Name = "NO₂", Concentration = components.No2.Value, Unit = "μg/m³" });

        if (components.O3.HasValue)
            pollutants.Add(new Pollutant { Name = "O₃", Concentration = components.O3.Value, Unit = "μg/m³" });

        if (components.So2.HasValue)
            pollutants.Add(new Pollutant { Name = "SO₂", Concentration = components.So2.Value, Unit = "μg/m³" });

        if (components.Pm2_5.HasValue)
            pollutants.Add(new Pollutant { Name = "PM2.5", Concentration = components.Pm2_5.Value, Unit = "μg/m³" });

        if (components.Pm10.HasValue)
            pollutants.Add(new Pollutant { Name = "PM10", Concentration = components.Pm10.Value, Unit = "μg/m³" });

        if (components.Nh3.HasValue)
            pollutants.Add(new Pollutant { Name = "NH₃", Concentration = components.Nh3.Value, Unit = "μg/m³" });

        return pollutants;
    }
}

