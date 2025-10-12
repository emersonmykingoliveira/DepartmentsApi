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

                var problem = new
                {
                    type = "https://tools.ietf.org/html/rfc7807",
                    title = "Invalid JSON",
                    detail = ex.Message,
                    status = 400
                };

                await JsonSerializer.SerializeAsync(httpContext.Response.Body, problem);
            }
        }
    }
}
