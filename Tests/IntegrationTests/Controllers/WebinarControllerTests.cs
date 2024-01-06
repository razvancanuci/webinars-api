using System.Net;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using Application.Requests;
using AutoFixture.Xunit2;
using Azure.Storage.Blobs;
using DataAccess;
using Domain.Entities;
using Domain.Settings;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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

    [Fact]
    public async Task AddWebinar_Returns_StatusCodeCreated()
    {
        // Arrange
        var requestData = new NewWebinarRequest
        {
            Title = "title",
            Host = "host",
            Description = "This is the Description",
            DateScheduled = DateTime.UtcNow.AddDays(7),
        };
        
        var apiVersion = "1.0";
        var httpClient = _factory.CreateClient();
        
        var stringDate = $"{requestData.DateScheduled.Month}/{requestData.DateScheduled.Day}/{requestData.DateScheduled.Year}";
        var requestContent = new MultipartFormDataContent();
        requestContent.Add(new StringContent(requestData.Title, Encoding.UTF8, MediaTypeNames.Text.Plain), "Title");
        requestContent.Add(new StringContent(requestData.Description, Encoding.UTF8, MediaTypeNames.Text.Plain), "Description");
        requestContent.Add(new StringContent(requestData.Host, Encoding.UTF8, MediaTypeNames.Text.Plain), "Host");
        requestContent.Add(new StringContent(stringDate, Encoding.UTF8, MediaTypeNames.Text.Plain), "DateScheduled");
        requestContent.Add(new StringContent(string.Empty, Encoding.UTF8, MediaTypeNames.Text.Plain), "Image");
        
        // Act
        var response = await httpClient.PostAsync(
            $"/api/v{apiVersion}/Webinar",
            requestContent);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Theory]
    [InlineData("1", "1.0")]
    [InlineData("2", "1.0")]
    public async Task RegisterToWebinar_Returns_StatusCodeNotFound(string webinarId, string apiVersion )
    {
        // Arrange
        var webinarRegistration = new Person
        {
            Email = "a@a.a",
            Name = "aaaaa",
        };
        var httpClient = _factory.CreateClient();

        // Act
        var response =
            await httpClient.PatchAsJsonAsync($"/api/v{apiVersion}/Webinar/{webinarId}", webinarRegistration);
        
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
        var blobSettings = sp.ServiceProvider.GetRequiredService<IOptions<AzureStorageSettings>>().Value;

        await dbContext.Database.EnsureDeletedAsync();
        await DeleteContainer(blobSettings);
    }

    private async Task DeleteContainer(AzureStorageSettings settings)
    {
        var blobServiceClient = new BlobServiceClient(settings.ConnectionString);
        var container = blobServiceClient.GetBlobContainerClient(settings.ContainerName);

        var containerExists = await container.ExistsAsync();

        if (containerExists)
        {
            await blobServiceClient.DeleteBlobContainerAsync(settings.ContainerName);
        }
    }
}