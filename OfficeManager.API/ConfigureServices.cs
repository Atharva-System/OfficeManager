using Microsoft.AspNetCore.Mvc;
using OfficeManager.API.Services;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Infrastructure.Persistence;

namespace OfficeManager.API
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddSingleton<ICurrentUserService, CurrentUserService>();

            services.AddHttpContextAccessor();

            services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();

            services.Configure<ApiBehaviorOptions>(options =>
                                options.SuppressModelStateInvalidFilter = true);

            services.AddOpenApiDocument(configure =>
            {
                configure.Title = "Office Manager API";
            });

            return services;
        }
    }
}
