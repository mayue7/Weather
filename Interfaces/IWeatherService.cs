using WeatherApi;

namespace Weather.Interfaces;

public interface IWeatherService
{
    Task<WeatherInfoResponse> GetCurrentWeatherByCountryByCity(string city, string country);
}


