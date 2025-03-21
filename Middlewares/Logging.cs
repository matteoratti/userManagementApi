using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

    public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Log the request
        _logger.LogInformation("HTTP Request: {Method} {Path}", context.Request.Method, context.Request.Path);

        // Call the next middleware in the pipeline
        await _next(context);

        // Log the response
        _logger.LogInformation("HTTP Response: {StatusCode}", context.Response.StatusCode);
    }
}