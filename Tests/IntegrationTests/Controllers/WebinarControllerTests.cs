using System.Net;
using System.Net.Http.Json;
using Application.Requests;
using AutoFixture.Xunit2;
using DataAccess;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.Controllers;

public class WebinarControllerTests : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    private readonly CustomWebApplicationFactory _factory;
    public WebinarControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("b21d1afb-7dc5-478f-b260-76e5f7af79ef", "1.0")]
    [InlineData("e6e56821-0284-4f10-aafa-167e1c8f5868", "1.0")]
    public async Task GetWebinarById_Returns_StatusCodeOK(string webinarId, string apiVersion)
    {
        // Arrange
        var httpClient = _factory.CreateClient();
        
        // Act
        var response = await httpClient.GetAsync($"/api/v{apiVersion}/Webinar/{webinarId}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Theory]
    [InlineData("e6e56821-0284-4f10-aafa-167e1c8f5862", "1.0")]
    public async Task GetWebinarById_Returns_StatusCodeNotFound(string webinarId, string apiVersion)
    {
        // Arrange
        var httpClient = _factory.CreateClient();
        
        // Act
        var response = await httpClient.GetAsync($"/api/v{apiVersion}/Webinar/{webinarId}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData("1.0", 1)]
    public async Task GetWebinars_Returns_StatusCodeOK(string apiVersion, int page)
    {
        // Arrange
        var httpClient = _factory.CreateClient();
        
        // Act
        var response = await httpClient.GetAsync($"/api/v{apiVersion}/Webinar?page={page}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [AutoData]
    public async Task AddWebinar_Returns_StatusCodeCreated(NewWebinarRequest webinar)
    {
        // Arrange
        var apiVersion = "1.0";
        var httpClient = _factory.CreateClient();
        
        // Act
        var response = await httpClient.PostAsJsonAsync($"/api/v{apiVersion}/Webinar", webinar);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
    
    [Theory]
    [AutoData]
    public async Task RegisterToWebinar_Returns_StatusCodeNotFound(RegisterWebinarRequest webinarRegistration)
    {
        // Arrange
        var apiVersion = "1.0";
        var httpClient = _factory.CreateClient();
        
        // Act
        var response = await httpClient.PatchAsJsonAsync($"/api/v{apiVersion}/Webinar", webinarRegistration);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    

    public async Task InitializeAsync()
    {
        using var sp = _factory.Services.CreateScope();
        var dbContext = sp.ServiceProvider.GetRequiredService<WebinarContext>();
        
        await dbContext.Database.EnsureCreatedAsync();
        DatabaseSeed.InitializeDbForTests(dbContext);
    }

    public async Task DisposeAsync()
    {
        using var sp = _factory.Services.CreateScope();
        var dbContext = sp.ServiceProvider.GetRequiredService<WebinarContext>();
        
        await dbContext.Database.EnsureDeletedAsync();
    }
}