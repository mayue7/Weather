using System.Net;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Dictionary<string, int> _requestCounts;
    private readonly int _limitPerHour = 5;
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
            await context.Response.WriteAsync("API key is missing.");
            return;
        }
        if (!IsValidApiKey(apiKey))
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync("Invalid API key ");
            return;
        }

        if (!IsRateLimitExceeded(apiKey))
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync("Rate limit exceeded. Please try again later.");
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
        return _requestCounts.TryGetValue(apiKey, out var count) ? count < _limitPerHour : true;
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