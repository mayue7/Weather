using Weather.Interfaces;
using WeatherApi;

namespace Weather.Services;

public class WeatherService: IWeatherService
{
    private readonly IHttpClientFactory _httpClientFactory;
    //private readonly string _apiKey;

    public WeatherService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        //_apiKey = apiKey;
    }

    public async Task<WeatherInfoResponse> GetCurrentWeatherByCountryByCity(string city, string country)
    {
        var httpClient = _httpClientFactory.CreateClient("WeatherApi");
        var apiUrl = $"weather?q={city},{country}&appid=8b7535b42fe1c551f18028f64e8688f7";

        return await httpClient.GetFromJsonAsync<WeatherInfoResponse>(apiUrl);
    }
}


