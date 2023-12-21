using System.Linq.Expressions;
using Application.Handlers.Queries;
using Application.Requests;
using Application.Services.Interfaces;
using AutoFixture.Xunit2;
using Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using StackExchange.Redis;

namespace Application.UnitTests.Handlers.Queries;

public class GetAvailableWebinarByIdQueryHandlerTests : RequestHandlerTestsBase<GetAvailableWebinarByIdQueryHandler>
{
    private readonly Mock<ICacheService> _cacheServiceMock;
    protected override GetAvailableWebinarByIdQueryHandler Handler { get; }
    public GetAvailableWebinarByIdQueryHandlerTests()
    {
        _cacheServiceMock = new();
        Handler = new (_cacheServiceMock.Object, UnitOfWorkMock.Object);
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
    public async Task Handle_ReturnsBadRequestResult_PassedThroughRepository(AvailableWebinarByIdRequest request)
    {
        // Arrange
        _cacheServiceMock.Setup(x => x.GetOrCreateAsync(
            It.IsAny<string>(),
            It.IsAny<Func<Task<IEnumerable<Webinar>>>>(),
            It.IsAny<TimeSpan>()
        )).ReturnsAsync(new List<Webinar> {new Webinar {ScheduleDate = DateTime.UtcNow.AddDays(-4)}});
        
        // Act
        var result = await Handler.Handle(request, CancellationToken.None);

        // Assert
        _cacheServiceMock.Verify(x => x.GetOrCreateAsync(
            It.IsAny<string>(),
            It.IsAny<Func<Task<IEnumerable<Webinar>>>>(),
            It.IsAny<TimeSpan>()
        ), Times.Once);
        result.Should().BeOfType<BadRequestObjectResult>();
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
        )).ReturnsAsync(new List<Webinar> {new Webinar {ScheduleDate = DateTime.UtcNow}});
        
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