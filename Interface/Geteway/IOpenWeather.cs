using WeatherCityAPI.Models;

namespace WeatherCityAPI.Interface.Geteway
{
    public interface IOpenWeather
    {

        public Task<List<GeocodingDTO>?> GetGeocodingDataAsync(string cityName);
        public Task<OpenWeatherWeatherDTO?> GetWeatherDataByCoordinatesAsync(double lat, double lon);
        public Task<OpenWeatherPollutionDTO?> GetAirPollutionDataAsync(double lat, double lon);
    }
}
