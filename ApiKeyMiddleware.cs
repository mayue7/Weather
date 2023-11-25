using System.Net;
using WeatherApi;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Dictionary<string, int> _requestCounts;
    private readonly IConfiguration _configuration;

    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _requestCounts = new Dictionary<string, int>();
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var apiKey = context.Request.Headers["x-api-key"].FirstOrDefault();

        if (string.IsNullOrEmpty(apiKey))
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsJsonAsync(new WeatherDescription(){
                Description = "API key is missing."
            });
            return;
        }
        if (!IsValidApiKey(apiKey))
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsJsonAsync(new WeatherDescription(){
                Description = "Invalid API key."
            });
            return;
        }

        if (!IsRateLimitExceeded(apiKey))
        {
            context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
            await context.Response.WriteAsJsonAsync(new WeatherDescription(){
                Description = "Rate limit exceeded. Please try again later."
            });
        }

        // Continue with the request if the API key is valid and rate limit has not been exceeded
        if (IsValidApiKey(apiKey) && IsRateLimitExceeded(apiKey))
        {
            IncrementRequestCount(apiKey);
            await _next(context);
        }
    }

    private bool IsValidApiKey(string apiKey)
    {
        var weatherApiKeys = _configuration.GetSection("WeatherApi:WeatherApiKeys").Get<string[]>();
        return !string.IsNullOrEmpty(apiKey) && weatherApiKeys != null && weatherApiKeys.Contains(apiKey);
    }

    private bool IsRateLimitExceeded(string apiKey)
    {
        var limitPerHourString = _configuration["ClientRateLimitPolicies:LimitPerHour"];
        return _requestCounts.TryGetValue(apiKey, out var count) ? count < Int32.Parse(limitPerHourString) : true;
    }

    private void IncrementRequestCount(string apiKey)
    {
        // Increment the request count for the API key
        if (_requestCounts.ContainsKey(apiKey))
        {
            _requestCounts[apiKey]++;
        }
        else
        {
            _requestCounts[apiKey] = 1;
        }
    }
}