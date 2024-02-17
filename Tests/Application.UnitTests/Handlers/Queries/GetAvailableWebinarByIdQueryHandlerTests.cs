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
    private readonly Mock<IFileStorage> _fileStorageMock;
    protected override GetAvailableWebinarByIdQueryHandler Handler { get; }
    public GetAvailableWebinarByIdQueryHandlerTests()
    {
        _cacheServiceMock = Fixture.Freeze<Mock<ICacheService>>();
        _fileStorageMock = Fixture.Freeze<Mock<IFileStorage>>();

        _fileStorageMock.Setup(m => m.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(default(Uri));
        
        Handler = Fixture.Create<GetAvailableWebinarByIdQueryHandler>();
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
        result.Should().BeOfType<NotFound<string>>();
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
            ScheduleDate = DateTime.UtcNow.AddDays(8),
        }});
        
        // Act
        var result = await Handler.Handle(request, CancellationToken.None);

        // Assert
        _cacheServiceMock.Verify(x => x.GetOrCreateAsync(
            It.IsAny<string>(),
            It.IsAny<Func<Task<IEnumerable<Webinar>>>>(),
            It.IsAny<TimeSpan>()
        ), Times.Once);
        result.Should().BeOfType<Ok<WebinarInfoDto>>();
    }
}