using System.Threading.RateLimiting;
using Application;
using Asp.Versioning;
using Azure.Identity;
using DataAccess;
using Domain.Settings;
using Microsoft.FeatureManagement;
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
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("Database"));
builder.Services.AddAzureAppConfiguration().AddFeatureManagement();
builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddProblemDetails();
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

app.MapControllers();

app.Run();


public partial class Program
{
}