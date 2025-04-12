using System.Net.Http.Headers;
using System.Text;

namespace spr311_web_api.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(ILogger<LoggingMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string space = "      ";
            var dt = DateTime.Now;

            // Request
            string logMessage = $"Request:\n" +
                $"{space}Method: {context.Request.Method}\n" +
                $"{space}IpAddress: {context.Connection.LocalIpAddress}\n" +
                $"{space}Path: {context.Request.Host + context.Request.Path}\n" +
                $"{space}Protocol: {context.Request.Protocol}\n" +
                $"{space}Date: {dt.ToLongTimeString()}.{dt.Millisecond}";

            _logger.LogInformation(logMessage);

            await _next(context);

            string headers = "";             

            foreach (var item in context.Response.Headers)
            {
                headers += $"{item.Key}: {item.Value}\n{space}";
            }

            dt = DateTime.Now;

            // Response
            logMessage = $"Response:\n" +
                $"{space}StatusCode: {context.Response.StatusCode}\n" +
                $"{space}Headers: {headers}" +
                $"Date: {dt.ToLongTimeString()}.{dt.Millisecond}";

            _logger.LogInformation(logMessage);
        }
    }
}
