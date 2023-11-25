using System;
using Xunit;
using Moq;
using AutoFixture;
using MediatR;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using WeatherApi;
using Weather.Commands;
using Weather.Controllers;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;

namespace ApiTests;

public class WeatherControllerTestss
{
    private readonly IFixture _fixture;
    private readonly Mock<IMediator> _mockMediator;

    public WeatherControllerTestss()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _mockMediator = _fixture.Freeze<Mock<IMediator>>();
    }

    [Theory, AutoData]
    public async Task GetCurrentWeatherRequestCommand_WithValidRequest_ReturnsOkResult_WithExpectedResults(
        WeatherDescription expected, string city, string country)
    {
        var command = _fixture
            .Build<GetCurrentWeatherRequestCommand>()
            .With(c => c.City, city)
            .With(c => c.Country, country)
            .Create();

        _mockMediator
            .Setup(m => m.Send(command, CancellationToken.None))
            .ReturnsAsync(expected);

        var controller = new WeatherController(_mockMediator.Object);

        var result = await controller.GetCurrentWeather(command) as OkObjectResult;

        result?.Value.Should().BeEquivalentTo(expected);
    }


    [Theory, AutoData]
    public async Task etCurrentWeatherRequestCommand_WithException_ReturnsInternalServerError(string city, string country)
    {
        var command = _fixture
            .Build<GetCurrentWeatherRequestCommand>()
            .With(c => c.City, city)
            .With(c => c.Country, country)
            .Create();

        _mockMediator
            .Setup(m => m.Send(command, CancellationToken.None))
            .ThrowsAsync(new Exception());

        var controller = new WeatherController(_mockMediator.Object);

        Func<Task> action = async () => await controller.GetCurrentWeather(command);

        await action.Should().ThrowAsync<Exception>();
    }
}