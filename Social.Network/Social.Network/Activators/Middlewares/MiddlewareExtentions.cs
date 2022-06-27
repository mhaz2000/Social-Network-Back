using Microsoft.AspNetCore.Builder;

namespace Social.Network.Activators.Middlewares
{
    public static class MiddlewareExtentions
    {
        public static void UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
