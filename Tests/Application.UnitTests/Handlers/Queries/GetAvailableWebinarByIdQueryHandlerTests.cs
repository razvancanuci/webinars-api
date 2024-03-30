using Application.Handlers.Queries;
using Application.Requests;
using Application.Services.Interfaces;
using AutoFixture;
using AutoFixture.Xunit2;
using Domain.Dtos;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace Application.UnitTests.Handlers.Queries;

public class GetAvailableWebinarByIdQueryHandlerTests : RequestHandlerTestsBase<GetAvailableWebinarByIdQueryHandler>
{
    private readonly Mock<ICacheService> _cacheServiceMock;
    protected override GetAvailableWebinarByIdQueryHandler Handler { get; }
    public GetAvailableWebinarByIdQueryHandlerTests()
    {
        _cacheServiceMock = Fixture.Freeze<Mock<ICacheService>>();
        var fileStorageMock = Fixture.Freeze<Mock<IFileStorage>>();

        fileStorageMock.Setup(m => m.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(default(Uri));
        
        Handler = Fixture.Create<GetAvailableWebinarByIdQueryHandler>();
    }

    [Theory]
    [AutoData]
    public async Task Handle_ReturnsNotFoundResult_PassedThroughRepository(AvailableWebinarByIdRequest request)
    {
        // Arrange
        _cacheServiceMock.Setup(m => m.GetOrCreateAsync(It.IsAny<string>(),
            It.IsAny<Func<ValueTask<Webinar>>>(),
            It.IsAny<TimeSpan?>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(default(Webinar)!);
        // Act
        var result = await Handler.Handle(request, CancellationToken.None);

        // Assert
        _cacheServiceMock.Verify(x => x.GetOrCreateAsync(
            It.IsAny<string>(),
            It.IsAny<Func<ValueTask<Webinar>>>(),
            It.IsAny<TimeSpan>(),
            It.IsAny<CancellationToken>()
        ), Times.Once);
        
        result.Should().BeOfType<NotFound<string>>();
    }
    
    [Theory]
    [AutoData]
    public async Task Handle_ReturnsOKResult_PassedThroughRepository(AvailableWebinarByIdRequest request)
    {
        // Arrange
        _cacheServiceMock.Setup(x => x.GetOrCreateAsync(
            It.IsAny<string>(),
            It.IsAny<Func<ValueTask<Webinar>>>(),
            It.IsAny<TimeSpan>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(
            new Webinar
        {
            Id= "1",
            Title = "Title",
            Description = "Description",
            Host = "Host",
            ScheduleDate = DateTime.UtcNow.AddDays(8),
        });
        
        // Act
        var result = await Handler.Handle(request, CancellationToken.None);

        // Assert
        _cacheServiceMock.Verify(x => x.GetOrCreateAsync(
            It.IsAny<string>(),
            It.IsAny<Func<ValueTask<Webinar>>>(),
            It.IsAny<TimeSpan>(),
            It.IsAny<CancellationToken>()
        ), Times.Once);
        result.Should().BeOfType<Ok<WebinarInfoDto>>();
    }
}