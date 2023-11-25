using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Weather.Commands;
using WeatherApi;
using AspNetCoreRateLimit;

namespace Weather.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WeatherController : ControllerBase
{
    private readonly IMediator mediator;

    public WeatherController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet("GetCurrentWeather")]
    [ProducesResponseType(typeof(WeatherDescription), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.TooManyRequests)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetCurrentWeather([FromQuery] GetCurrentWeatherRequestCommand query)
    {
        var response = await mediator.Send(query);
        return Ok(response);
    }
}