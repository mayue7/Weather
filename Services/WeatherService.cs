using Weather.Interfaces;
using WeatherApi;

namespace Weather.Services;

public class WeatherService : IWeatherService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public WeatherService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<WeatherInfoResponse> GetCurrentWeatherByCountryByCity(string city, string country)
    {
        var httpClient = _httpClientFactory.CreateClient("WeatherApi");
        string openWeatherApiKey = _configuration["OpenWeatherMap:OpenWeatherApiKey"];
        var apiUrl = $"weather?q={city},{country}&appid={openWeatherApiKey}";

        return await httpClient.GetFromJsonAsync<WeatherInfoResponse>(apiUrl);
    }
}
