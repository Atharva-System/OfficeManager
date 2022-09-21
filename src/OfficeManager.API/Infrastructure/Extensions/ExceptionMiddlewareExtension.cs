using OfficeManager.API.Infrastructure.Middleware;

namespace OfficeManager.API.Infrastructure.Extensions
{
    public static class ExceptionMiddlewareExtension
    {
        public static void UseCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
