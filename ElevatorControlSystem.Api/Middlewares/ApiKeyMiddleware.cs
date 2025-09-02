using Microsoft.Extensions.Options;

namespace ElevatorControlSystem.Api;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private const string APIKEY_HEADER_NAME = "X-API-KEY";
    private readonly string _apiKey;
    public ApiKeyMiddleware(RequestDelegate next, IOptions<ApiSettings> options)
    {
        this._next = next;
        _apiKey = options.Value.ApiKey;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/swagger"))
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue(APIKEY_HEADER_NAME, out var extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API key is not found in the header");
            return;
        }
        if (!_apiKey.Equals(extractedApiKey))
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Unauthorized Client");
            return;
        }
        await _next(context);
    }

}
