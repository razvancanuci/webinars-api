using Application;
using Asp.Versioning;
using Azure.Identity;
using DataAccess;
using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (!string.IsNullOrEmpty(builder.Configuration["AppConfig:ConnectionString"]) && !string.IsNullOrEmpty(builder.Configuration["KeyVault:Uri"]))
{
    builder.Configuration.AddAzureAppConfiguration(builder.Configuration["AppConfig:ConnectionString"]);
    builder.Configuration.AddAzureKeyVault(new Uri(builder.Configuration["KeyVault:Uri"] ?? string.Empty), new DefaultAzureCredential());
}

builder.Services.AddAzureAppConfiguration().AddFeatureManagement();

builder.Services.AddApplicationServices().AddDataAccess(builder.Configuration);
builder.Services.AddControllers();

builder.Services.AddMvc();
builder.Services.AddApiVersioning(o =>
{
    o.AssumeDefaultVersionWhenUnspecified = true ;
    o.DefaultApiVersion = new ApiVersion( 1 );
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.Services.AutoCreateDb();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();


public partial class Program { }
