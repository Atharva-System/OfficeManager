using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OfficeManager.API;
using OfficeManager.API.Infrastructure.Filters;
using OfficeManager.Application;
using OfficeManager.Infrastructure;
using OfficeManager.Infrastructure.Persistence;
using OfficeManager.Infrastructure.Settings;
using System.Text;
using WatchDog;
using WatchDog.src.Enums;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureService(builder.Configuration);
builder.Services.AddApiServices();
builder.Services.AddCors();
var jwtSettings = builder.Configuration.GetSection("JWTSettings").Get<JWTSettings>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience[0],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecurityKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

var policy = new AuthorizationPolicyBuilder()
.RequireAuthenticatedUser()
.Build();

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new AuthorizeFilter(policy));
    options.Filters.Add(new ApiValidationExceptionFilter());
    options.Filters.Add(new AccessExceptionFilter());
    options.Filters.Add(new NotFoundExceptionFilter());
    options.Filters.Add(new AccessExceptionFilter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//swagger
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Office Manager - Api", Version = "v1", });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
            Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{ }
        }
    });
});

var postgreSqlConnection = builder.Configuration.GetConnectionString("PostgreSqlConnection");
if (!string.IsNullOrEmpty(postgreSqlConnection))
{
    //WatchDog
    builder.Services.AddWatchDogServices(opt =>
    {
        opt.IsAutoClear = false;
        // opt.ClearTimeSchedule = WatchDogAutoClearScheduleEnum.Weekly;
        opt.SetExternalDbConnString = postgreSqlConnection;
        opt.SqlDriverOption = WatchDogSqlDriverEnum.PostgreSql;
    });
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();

    using (var scope = app.Services.CreateScope())
    {
        var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
        await initializer.InitializeAsync();
        await initializer.SeedAsync();
    }

    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OfficeManagerAPI v1"));
}
else
{
    app.UseHsts();
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();

var resourcesPath = Path.Combine(Directory.GetCurrentDirectory(), @"Resources");
if (!Directory.Exists(resourcesPath))
{
    Directory.CreateDirectory(resourcesPath);
}

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(resourcesPath),
    RequestPath = new PathString("/Resources")
});

app.UseCors(x => x.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod());

//app.UseSwaggerUi3(settings =>
//{
//    settings.Path = "/api";
//    settings.DocumentPath = "/api/specification.json";
//});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
InitializeDatabase(app);
app.MapFallbackToFile("index.html"); ;

if (!string.IsNullOrEmpty(postgreSqlConnection))
{
    var watchDogSettings = builder.Configuration.GetSection("WatchDogSettings").Get<WatchDogSettings>();

    // inject into the middleware
    app.UseWatchDogExceptionLogger();

    if (!string.IsNullOrEmpty(watchDogSettings.Username)
        && !string.IsNullOrEmpty(watchDogSettings.Password))
    {
        app.UseWatchDog(opt =>
        {
            opt.WatchPageUsername = watchDogSettings.Username;
            opt.WatchPagePassword = watchDogSettings.Password;
        });
    }
}
app.Run();

void InitializeDatabase(IApplicationBuilder app)
{
#pragma warning disable CS8602 // Dereference of a possibly null reference.
    using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
    {
        serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
    }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
}