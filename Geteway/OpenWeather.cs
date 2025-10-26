using RestSharp;
using System.Text.Json;
using WeatherCityAPI.Interface.Geteway;
using WeatherCityAPI.Models;

namespace WeatherCityAPI.GetWay
{



    public class OpenWeather : IOpenWeather
    {
        private readonly RestClient _client;
        private readonly string _apiKey;

        public OpenWeather(IConfiguration configuration)
        {
            var apiKey = configuration["OpenWeatherMap:ApiKey"];
            var baseUrl = configuration["OpenWeatherMap:BaseUrl"] ?? "https://api.openweathermap.org";

            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentNullException(nameof(apiKey), "OpenWeatherMap API Key is not configured");
            }

            _apiKey = apiKey;
            _client = new RestClient(baseUrl);
        }
        public async Task<List<GeocodingDTO>?> GetGeocodingDataAsync(string cityName)
        {
            var request = new RestRequest("/geo/1.0/direct");
            request.AddParameter("q", cityName);
            request.AddParameter("limit", 1);
            request.AddParameter("appid", _apiKey);

            var response = await _client.ExecuteAsync(request);

            if (!response.IsSuccessStatusCode || string.IsNullOrEmpty(response.Content))
            {

                var failedResponse = JsonSerializer.Deserialize<ServiceResultDTO>(response.Content);
                switch (failedResponse.cod)
                {
                    case 400:
                        throw new Exception("Error 400 - Bad Request. Check your API parameters.");

                    case 401:
                        throw new Exception("Error 401 - Unauthorized. Invalid API key.");

                    case 404:
                        throw new Exception("Error 404 - Not Found. City not found or invalid location.");

                    case 429:
                        throw new Exception("Error 429 - Rate Limit Exceeded. Too many requests.");

                    default:
                        if (failedResponse.cod >= 500)
                            throw new Exception("Error 5xx - Server Error. OpenWeatherMap API server issue.");
                        else
                            throw new Exception($"API Error: {failedResponse.cod}");
                }
            }
            return JsonSerializer.Deserialize<List<GeocodingDTO>>(response.Content);

        }
        public async Task<OpenWeatherWeatherDTO?> GetWeatherDataByCoordinatesAsync(double lat, double lon)
        {
            var request = new RestRequest("/data/2.5/weather");
            request.AddParameter("lat", lat);
            request.AddParameter("lon", lon);
            request.AddParameter("appid", _apiKey);
            request.AddParameter("units", "metric");

            var response = await _client.ExecuteAsync(request);

            if (!response.IsSuccessStatusCode || string.IsNullOrEmpty(response.Content))
            {
                var failedResponse = JsonSerializer.Deserialize<ServiceResultDTO>(response.Content);
                switch (failedResponse.cod)
                {
                    case 400: 
                        throw new Exception("Error 400 - Bad Request. Check your API parameters.");

                    case 401:
                        throw new Exception("Error 401 - Unauthorized. Invalid API key.");

                    case 404:
                        throw new Exception("Error 404 - Not Found. City not found or invalid location.");

                    case 429:
                        throw new Exception("Error 429 - Rate Limit Exceeded. Too many requests.");

                    default:
                        if (failedResponse.cod >= 500)
                            throw new Exception("Error 5xx - Server Error. OpenWeatherMap API server issue.");
                        else
                            throw new Exception($"API Error: {failedResponse.cod}");
                }
            }

            return JsonSerializer.Deserialize<OpenWeatherWeatherDTO>(response.Content);
        }
        public async Task<OpenWeatherPollutionDTO?> GetAirPollutionDataAsync(double lat, double lon)
        {
            var request = new RestRequest("/data/2.5/air_pollution");
            request.AddParameter("lat", lat);
            request.AddParameter("lon", lon);
            request.AddParameter("appid", _apiKey);
            request.AddParameter("lang", "fa");

            var response = await _client.ExecuteAsync(request);

            if (!response.IsSuccessStatusCode || string.IsNullOrEmpty(response.Content))
            {
                var failedResponse = JsonSerializer.Deserialize<ServiceResultDTO>(response.Content);
                switch (failedResponse.cod)
                {
                    case 400: 
                        throw new Exception("Error 400 - Bad Request. Check your API parameters.");

                    case 401:
                        throw new Exception("Error 401 - Unauthorized. Invalid API key.");

                    case 404:
                        throw new Exception("Error 404 - Not Found. City not found or invalid location.");

                    case 429:
                        throw new Exception("Error 429 - Rate Limit Exceeded. Too many requests.");

                    default:
                        if (failedResponse.cod >= 500)
                            throw new Exception("Error 5xx - Server Error. OpenWeatherMap API server issue.");
                        else
                            throw new Exception($"API Error: {failedResponse.cod}");
                }
            }

            return JsonSerializer.Deserialize<OpenWeatherPollutionDTO>(response.Content);
        }
    }
}
