using System.Threading;
using System.Threading.Tasks;
using Moq;
using Weather.Commands;
using Weather.Interfaces;
using WeatherApi;
using Xunit;
using AutoFixture;

using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;

public class GetCurrentWeatherRequestCommandHandlerTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IWeatherService> _mockWeatherService;

    public GetCurrentWeatherRequestCommandHandlerTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _mockWeatherService = _fixture.Freeze<Mock<IWeatherService>>();
    }

    [Fact]
    public async Task Handle_Returns_WeatherDescription_With_ValidRequest()
    {
        // Arrange
        _mockWeatherService.Setup(service => service.GetCurrentWeatherByCountryByCity(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new WeatherDescription { Description = "Cloudy" });

        var handler = new GetCurrentWeatherRequestCommandHandler(_mockWeatherService.Object);
        var request = new GetCurrentWeatherRequestCommand { City = "city", Country = "country" };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Cloudy", result.Description);
    }

    [Fact]
    public async Task Handle_Returns_NullWeatherDescription_When_WeatherServiceReturnsNull()
    {
        // Arrange
        _mockWeatherService.Setup(service => service.GetCurrentWeatherByCountryByCity(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(null as WeatherDescription);

        var handler = new GetCurrentWeatherRequestCommandHandler(_mockWeatherService.Object);
        var request = new GetCurrentWeatherRequestCommand { City = "city", Country = "country" };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}
