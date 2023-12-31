using System;
using MediatR;
using Weather.Interfaces;
using WeatherApi;

namespace Weather.Commands;

public class GetCurrentWeatherRequestCommand : IRequest<WeatherDescription>
{
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}

public class GetCurrentWeatherRequestCommandHandler : IRequestHandler<GetCurrentWeatherRequestCommand, WeatherDescription>
{
    private readonly IWeatherService _weatherService;

    public GetCurrentWeatherRequestCommandHandler(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    public async Task<WeatherDescription> Handle(GetCurrentWeatherRequestCommand query, CancellationToken cancellationToken)
    {
        return await _weatherService.GetCurrentWeatherByCountryByCity(query.City, query.Country);
    }
}