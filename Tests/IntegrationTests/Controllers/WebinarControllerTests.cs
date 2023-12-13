using System.Net;
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