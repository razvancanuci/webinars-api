using System.Threading.RateLimiting;
using Application;
using Asp.Versioning;
using Azure.Identity;
using DataAccess;
using Domain.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.FeatureManagement;
using StackExchange.Redis;
using WebAPI.Handlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (!string.IsNullOrEmpty(builder.Configuration["AppConfig:ConnectionString"]) &&
    !string.IsNullOrEmpty(builder.Configuration["KeyVault:Uri"]))
{
    builder.Configuration.AddAzureAppConfiguration(builder.Configuration["AppConfig:ConnectionString"]);
    builder.Configuration.AddAzureKeyVault(new Uri(builder.Configuration["KeyVault:Uri"] ?? string.Empty),
        new DefaultAzureCredential());
}

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policyBuilder => policyBuilder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});
builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("fixed-by-ip", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 10,
                Window = TimeSpan.FromMinutes(1)
            }));
});
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(builder.Configuration["Redis:ConnectionString"] ?? string.Empty));

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("Database"));
builder.Services.AddAzureAppConfiguration().AddFeatureManagement();
builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddHealthChecks()
    .AddRedis(builder.Configuration["Redis:ConnectionString"] ?? string.Empty)
    .AddDbContextCheck<WebinarContext>(
        "cosmosDbHealthCheck",
        HealthStatus.Unhealthy,
        customTestQuery: async (context, _) =>
        {
            try
            {
                await context.Database.GetCosmosClient().ReadAccountAsync().ConfigureAwait(false);
            }
            catch (HttpRequestException)
            {
                return false;
            }
            return true;
        });

builder.Services.AddApplicationInsightsTelemetry(options =>
{
    options.ConnectionString = builder.Configuration["AppInsights:ConnectionString"] ?? string.Empty;
});
builder.Services.AddApplicationServices().AddDataAccess();
builder.Services.AddControllers();

builder.Services.AddMvc();
builder.Services.AddApiVersioning(o =>
{
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new ApiVersion(1);
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.Services.AutoCreateDb();
}

app.UseCors();
app.UseHttpsRedirection();
app.UseExceptionHandler();
app.UseRateLimiter();

app.MapHealthChecks("/health");
app.MapControllers();

app.Run();


public partial class Program
{
}