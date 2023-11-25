using System.Collections.Concurrent;
using System.Net;
using WeatherApi;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    private readonly ConcurrentDictionary<string, RateLimitInfo> _rateLimits;

    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
        _rateLimits = new ConcurrentDictionary<string, RateLimitInfo>();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var apiKey = context.Request.Headers["x-api-key"].FirstOrDefault();
        var limitPerHourString = _configuration["ClientRateLimitPolicies:LimitPerHour"];
        var limitPerHour = Int32.Parse(limitPerHourString);

        if (string.IsNullOrEmpty(apiKey))
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsJsonAsync(new WeatherDescription()
            {
                Description = "API key is missing."
            });
            return;
        }
        if (!IsValidApiKey(apiKey))
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsJsonAsync(new WeatherDescription()
            {
                Description = "Invalid API key."
            });
            return;
        }

        if (!_rateLimits.TryGetValue(apiKey, out var rateLimitInfo))
        {
            rateLimitInfo = new RateLimitInfo
            {
                LastRequestTime = DateTime.UtcNow,
                Requests = 0
            };
            _rateLimits.TryAdd(apiKey, rateLimitInfo);
        }

        var currentTime = DateTime.UtcNow;
        var timeSinceLastRequest = currentTime - rateLimitInfo.LastRequestTime;

        if (timeSinceLastRequest < TimeSpan.FromHours(1) && rateLimitInfo.Requests >= limitPerHour)
        {
            context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
            await context.Response.WriteAsJsonAsync(new WeatherDescription()
            {
                Description = "Rate limit exceeded. Please try again later."
            });
            return;
        }

        rateLimitInfo.LastRequestTime = currentTime;
        rateLimitInfo.Requests++;

        await _next(context);
    }

    private bool IsValidApiKey(string apiKey)
    {
        var weatherApiKeys = _configuration.GetSection("WeatherApi:WeatherApiKeys").Get<string[]>();
        return !string.IsNullOrEmpty(apiKey) && weatherApiKeys != null && weatherApiKeys.Contains(apiKey);
    }
}

public class RateLimitInfo
{
    public DateTime LastRequestTime { get; set; }
    public int Requests { get; set; }
}