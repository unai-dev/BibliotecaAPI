namespace BibliotecaAPI.Middlewares
{
    public class LoggerMiddleware
    {
        private readonly RequestDelegate next;

        public LoggerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path}");

            await next.Invoke(context);
            logger.LogInformation($"Response: {context.Response.StatusCode}");
        }
    }

    public static class LoggerRequestMiddlewareExtension
    {
        public static IApplicationBuilder UseLoggerRequest(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggerMiddleware>();
        }
    }
}
