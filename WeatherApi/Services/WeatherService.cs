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

    public async Task<WeatherDescription> GetCurrentWeatherByCountryByCity(string city, string country)
    {
        var httpClient = _httpClientFactory.CreateClient("WeatherApi");
        string openWeatherApiKey = _configuration["OpenWeatherMap:OpenWeatherApiKey"];
        var apiUrl = $"weather?q={city},{country}&appid={openWeatherApiKey}";

        var weatherResponse = new WeatherDescription();
        try {
            var response = await httpClient.GetFromJsonAsync<WeatherInfoResponse>(apiUrl);
            if (response != null && response.Weather != null) 
            {
                weatherResponse.Description = response.Weather[0].Description;
                return weatherResponse;
            } 
        } 
        catch (Exception e)
        {
            weatherResponse.Description = $"Got Exception: ${e}";
        }
        return weatherResponse;
    }
}

