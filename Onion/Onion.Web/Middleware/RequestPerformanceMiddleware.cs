using System.Diagnostics;

namespace Onion.Web.Middleware
{
    public class RequestPerformanceMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestPerformanceMiddleware> _logger;

        public RequestPerformanceMiddleware(RequestDelegate next, ILogger<RequestPerformanceMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            // Pass execution to the next middleware in the pipeline
            await _next(context);

            stopwatch.Stop();

            _logger.LogInformation(
                "HTTP {Method} {Path} responded {StatusCode} in {ElapsedMs}ms",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds);
        }
    }

    // Extension method for clean registration in Program.cs -> app.UseRequestPerformance();
    public static class PerformanceMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestPerformance(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestPerformanceMiddleware>();
        }
    }
}
