using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Domain.Entities;
using OfficeManager.Infrastructure.Identity;
using OfficeManager.Infrastructure.Persistence;
using OfficeManager.Infrastructure.Persistence.Interceptors;
using OfficeManager.Infrastructure.Services;

namespace OfficeManager.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<AuditableEntitySaveChangesInterceptor>();

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("Default"),
                builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddIdentity<ApplicationUser,IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
            services.AddScoped<IContextServices>(provider => provider.GetRequiredService<ContextServices>());

            services.AddScoped<ApplicationDbContextInitializer>();

            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<IIdentityService, IdentityService>();

            return services;
        }
    }
}
