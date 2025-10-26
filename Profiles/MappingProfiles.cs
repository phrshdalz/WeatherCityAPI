using AutoMapper;
using WeatherCityAPI.Models;

namespace WeatherCityAPI.Profiles
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {

            CreateMap<OpenWeatherWeatherDTO, WeatherCityResponse>()
                .ForMember(x => x.TemperatureCelsius, c => c.MapFrom(Z => Z.Main.Temp + " C"))
                .ForMember(x => x.Humidity, c => c.MapFrom(Z => Z.Main.Humidity + " %"))
                .ForMember(x => x.WindSpeed, c => c.MapFrom(Z => Z.Wind.Speed + " m/s"));


        }
    }
}
