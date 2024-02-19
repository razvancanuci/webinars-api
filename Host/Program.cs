using System.Threading.RateLimiting;
using Application;
using Asp.Versioning;
using Azure.Identity;
using AzureStorage;
using DataAccess;
using Domain.Messages;
using Domain.Settings;
using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using Microsoft.Identity.Web;
using WebAPI.Endpoints;
using WebAPI.Handlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (!string.IsNullOrEmpty(builder.Configuration["AppConfig:ConnectionString"]))
{
    builder.Configuration.AddAzureAppConfiguration(options =>
    {
        options.Connect(
                builder.Configuration["AppConfig:ConnectionString"])
            .ConfigureKeyVault(kv =>
            {
                kv.SetCredential(new DefaultAzureCredential());
            });
    });
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

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:ConnectionString"];
});

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("Database"));
builder.Services.Configure<AzureStorageSettings>(builder.Configuration.GetSection("Storage"));
builder.Services.Configure<AzureServiceBusSettings>(builder.Configuration.GetSection("ServiceBus"));

builder.Services.AddAzureAppConfiguration().AddFeatureManagement();
builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    
    x.AddConfigureEndpointsCallback((name, cfg) =>
    {
        cfg.UseMessageRetry(r => r.Immediate(2));
    });
    
    x.UsingAzureServiceBus((context, cfg) =>
    {
        cfg.Host(builder.Configuration["ServiceBus:ConnectionString"]);
        
        cfg.Message<EmailRegistrationMessage>(
            x =>
            {
                x.SetEntityName(builder.Configuration["ServiceBus:SendEmailTopicName"]!);
            });
        
        cfg.Message<EmailCancellationMessage>(
            x =>
            {
                x.SetEntityName(builder.Configuration["ServiceBus:SendEmailTopicName"]!);
            });
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddHealthChecks()
    .AddRedis(builder.Configuration["Redis:ConnectionString"]!)
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
        })
    .AddAzureBlobStorage(
        builder.Configuration["Storage:ConnectionString"]!,
        builder.Configuration["Storage:ContainerName"]!
    ).AddAzureServiceBusTopic(sp =>
    {
        var settings = sp.GetRequiredService<IOptions<AzureServiceBusSettings>>().Value;
        return settings.ConnectionString;
    }, sp =>
    {
        var settings = sp.GetRequiredService<IOptions<AzureServiceBusSettings>>().Value;
        return settings.SendEmailTopicName;
    });

builder.Services.AddApplicationInsightsTelemetry(options =>
{
    options.ConnectionString = builder.Configuration["AppInsights:ConnectionString"]!;
});

builder.Services.AddApplicationServices()
    .AddDataAccess()
    .AddAzureBlobStorage();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(options =>
    {
        builder.Configuration.Bind("AzureAdB2C", options);
    },
    options =>
    {
        builder.Configuration.Bind("AzureAdB2C", options);
    });

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

app.UseAuthentication();
app.UseAuthorization();

app.UseCors();
app.UseHttpsRedirection();
app.UseExceptionHandler();
app.UseRateLimiter();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

var versionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1))
    .Build();

app.NewApiVersionSet();
app.AddWebinarEndpoints(versionSet);

app.Run();


public partial class Program
{
}