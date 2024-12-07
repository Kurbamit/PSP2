using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ReactApp1.Server.Exceptions;

namespace ReactApp1.Server.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;
        
        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                
                response.StatusCode = error switch
                {
                    BaseException e => (int)e.StatusCode,
                    _ => StatusCodes.Status500InternalServerError,
                };
                
                var problemDetails = new ProblemDetails
                {
                    Status = response.StatusCode,
                    Title = error.Message,
                };
                
                _logger.LogError(error.Message);
                var result = JsonSerializer.Serialize(problemDetails);
                await response.WriteAsync(result);
            }
        }
    }
}