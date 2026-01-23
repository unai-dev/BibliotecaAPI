namespace BibliotecaAPI.Middlewares
{
    public class BlockedPathMiddleware
    {
        private readonly RequestDelegate next;

        public BlockedPathMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            if (context.Request.Path == "/bloqueado")
            {
                context.Response.StatusCode = 403;

                await context.Response.WriteAsync("Access Denied");
            }
            else
            {
                await next.Invoke(context);
            }

        }
    }

    public static class BlockedPathMiddlewareExtension
    {
        public static IApplicationBuilder UseBlockedPath(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BlockedPathMiddleware>();
        }

    }
}
