using System;
using MediatR;
using Weather.Interfaces;
using WeatherApi;

namespace Weather.Commands;

public class GetCurrentWeatherRequestCommand: IRequest<WeatherInfoResponse>
{
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}

public class GetCurrentWeatherRequestCommandHandler: IRequestHandler<GetCurrentWeatherRequestCommand, WeatherInfoResponse>
{
    private readonly IMediator _mediator;
    private readonly IWeatherService _weatherService;

    public GetCurrentWeatherRequestCommandHandler(IMediator mediator, IWeatherService weatherService)
    {
        _mediator = mediator;
        _weatherService = weatherService;
    }

    public async Task<WeatherInfoResponse> Handle(GetCurrentWeatherRequestCommand query, CancellationToken cancellationToken)
    {
        return await _weatherService.GetCurrentWeatherByCountryByCity(query.City, query.Country);
    }
}