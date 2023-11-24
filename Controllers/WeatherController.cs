using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Weather.Commands;
using WeatherApi;

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
    [ProducesResponseType(typeof(WeatherInfoResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.TooManyRequests)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetCurrentWeather([FromQuery] GetCurrentWeatherRequestCommand query)
    {
        var response = await mediator.Send(query);
        if (response == null)
        {
            // Handle rate limit exceeded response
            return StatusCode((int)HttpStatusCode.TooManyRequests, "Rate limit exceeded. Please try again later.");
        }
        return Ok(response);
    }
}