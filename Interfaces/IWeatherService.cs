using WeatherApi;

namespace Weather.Interfaces;

public interface IWeatherService
{
    Task<WeatherDescription> GetCurrentWeatherByCountryByCity(string city, string country);
}


