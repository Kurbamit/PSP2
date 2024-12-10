namespace ReactApp1.Server.Middlewares;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;
    
    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        // Log request details
        _logger.LogInformation("Handling request: {Method} {Path}", context.Request.Method, context.Request.Path);

        await _next(context); // Call the next middleware in the pipeline

        // Log response details
        _logger.LogInformation("Response: {StatusCode}", context.Response.StatusCode);
    }
}