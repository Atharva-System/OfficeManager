using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EasyCaching.Core.Configurations;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Infrastructure.Persistence;
using OfficeManager.Infrastructure.Persistence.Interceptors;
using OfficeManager.Infrastructure.Services;
using OfficeManager.Infrastructure.Settings;

namespace OfficeManager.Infrastructure
{
    public static class InfrastructureRegistration
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<AuditableEntitySaveChangesInterceptor>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("Default"),
                builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
            services.AddScoped<ApplicationDbContextInitializer>();
            services.AddTransient<IDateTime, DateTimeService>();
            services.AddSingleton<ICurrentUserServices, CurrentUserService>();
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.Configure<CacheSettings>(configuration.GetSection("CacheSettings"));
            services.AddTransient<IFilesServices, FilesServices>();
            services.AddScoped<ITokenService, TokenService>();
            services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));

            var cacheSettings = configuration.GetSection("CacheSettings").Get<CacheSettings>();

            if (cacheSettings.PreferRedis)
            {
                services.AddEasyCaching(option =>
                {
                    option.WithJson();
                    option.UseRedis(config =>
                    {
                        config.DBConfig.Endpoints.Add(new ServerEndPoint(cacheSettings.RedisURL, cacheSettings.RedisPort));
                    }, "json");
                });
            }
            else
            {
                services.AddEasyCaching(option =>
                {
                    option.UseInMemory();
                });
            }
            services.AddTransient<IEasyCacheService, EasyCacheService>();

            return services;
        }
    }
}
