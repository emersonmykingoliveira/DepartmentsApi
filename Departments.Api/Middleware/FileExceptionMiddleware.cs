using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Departments.Api.Middleware
{
    public sealed class FileExceptionMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly ILogger<FileExceptionMiddleware> _logger;

        public FileExceptionMiddleware(RequestDelegate requestDelegate, ILogger<FileExceptionMiddleware> logger)
        {
            _requestDelegate = requestDelegate;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _requestDelegate(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error parsing file");

                var problem = new ProblemDetails
                {
                    Title = "Error parsing file",
                    Detail = ex.Message,
                };

                await JsonSerializer.SerializeAsync(httpContext.Response.Body, problem, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }
        }
    }
}
