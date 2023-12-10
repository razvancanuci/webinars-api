using System.Net;
using FluentAssertions;

namespace IntegrationTests.Controllers;

public class WebinarControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;
    public WebinarControllerTests(CustomWebApplicationFactory factory)
    {
        _httpClient = factory.CreateClient();
    }

    [Theory]
    [InlineData("b21d1afb-7dc5-478f-b260-76e5f7af79ef", "1.0")]
    [InlineData("e6e56821-0284-4f10-aafa-167e1c8f5868", "1.0")]
    public async Task GetWebinarById_Returns_StatusCodeOK(string webinarId, string apiVersion)
    {
        // Act
        var response = await _httpClient.GetAsync($"/api/v{apiVersion}/Webinar/{webinarId}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}