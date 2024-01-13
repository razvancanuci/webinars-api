using System.Linq.Expressions;
using Application.Handlers.Queries;
using Application.Requests;
using Application.Services.Interfaces;
using AutoFixture.Xunit2;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using StackExchange.Redis;

namespace Application.UnitTests.Handlers.Queries;

public class GetAvailableWebinarByIdQueryHandlerTests : RequestHandlerTestsBase<GetAvailableWebinarByIdQueryHandler>
{
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly Mock<IFileStorage> _fileStorageMock;
    protected override GetAvailableWebinarByIdQueryHandler Handler { get; }
    public GetAvailableWebinarByIdQueryHandlerTests()
    {
        _cacheServiceMock = new();
        _fileStorageMock = new();
        Handler = new (_cacheServiceMock.Object, _fileStorageMock.Object, UnitOfWorkMock.Object);
    }

    [Theory]
    [AutoData]
    public async Task Handle_ReturnsNotFoundResult_PassedThroughRepository(AvailableWebinarByIdRequest request)
    {
        // Arrange
        _cacheServiceMock.Setup(x => x.GetOrCreateAsync(
            It.IsAny<string>(),
            It.IsAny<Func<Task<IEnumerable<Webinar>>>>(),
            It.IsAny<TimeSpan>()
        )).ReturnsAsync(new List<Webinar>());
        
        // Act
        var result = await Handler.Handle(request, CancellationToken.None);

        // Assert
        _cacheServiceMock.Verify(x => x.GetOrCreateAsync(
            It.IsAny<string>(),
            It.IsAny<Func<Task<IEnumerable<Webinar>>>>(),
            It.IsAny<TimeSpan>()
        ), Times.Once);
        result.Should().BeOfType<NotFoundObjectResult>();
    }
    
    [Theory]
    [AutoData]
    public async Task Handle_ReturnsOKResult_PassedThroughRepository(AvailableWebinarByIdRequest request)
    {
        // Arrange
        _cacheServiceMock.Setup(x => x.GetOrCreateAsync(
            It.IsAny<string>(),
            It.IsAny<Func<Task<IEnumerable<Webinar>>>>(),
            It.IsAny<TimeSpan>()
        )).ReturnsAsync(new List<Webinar> {
            new Webinar
        {
            Id= "1",
            Title = "Title",
            Description = "Description",
            Host = "Host",
            ScheduleDate = DateTime.UtcNow.AddDays(8)
        }});
        
        // Act
        var result = await Handler.Handle(request, CancellationToken.None);

        // Assert
        _cacheServiceMock.Verify(x => x.GetOrCreateAsync(
            It.IsAny<string>(),
            It.IsAny<Func<Task<IEnumerable<Webinar>>>>(),
            It.IsAny<TimeSpan>()
        ), Times.Once);
        result.Should().BeOfType<OkObjectResult>();
    }
}