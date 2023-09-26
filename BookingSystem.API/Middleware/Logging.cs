namespace BookingSystem.API.Middleware
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Capture the request body for logging purposes
            context.Request.EnableBuffering();
            var requestBodyStream = new MemoryStream();
            await context.Request.Body.CopyToAsync(requestBodyStream);
            requestBodyStream.Seek(0, SeekOrigin.Begin);
            var requestBodyText = new StreamReader(requestBodyStream).ReadToEnd();
            context.Request.Body.Seek(0, SeekOrigin.Begin);

            // Log the request
            _logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path}");
            _logger.LogInformation($"Request Headers: {string.Join(", ", context.Request.Headers)}");
            _logger.LogInformation($"Request Body: {requestBodyText}");

            // Capture the response body for logging purposes
            var originalResponseBody = context.Response.Body;
            using var responseBodyStream = new MemoryStream();
            context.Response.Body = responseBodyStream;

            // Call the next middleware in the pipeline
            await _next(context);

            // Log the response
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            var responseBodyText = new StreamReader(responseBodyStream).ReadToEnd();
            _logger.LogInformation($"Response: {context.Response.StatusCode}");
            _logger.LogInformation($"Response Headers: {string.Join(", ", context.Response.Headers)}");
            _logger.LogInformation($"Response Body: {responseBodyText}");

            // Copy the response back to the original stream
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            await responseBodyStream.CopyToAsync(originalResponseBody);
        }
    }

}
