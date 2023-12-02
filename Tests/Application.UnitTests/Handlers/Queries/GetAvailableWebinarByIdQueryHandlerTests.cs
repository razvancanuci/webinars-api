using System.Linq.Expressions;
using Application.Handlers.Queries;
using Application.Requests;
using AutoFixture.Xunit2;
using Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Application.UnitTests.Handlers.Queries;

public class GetAvailableWebinarByIdQueryHandlerTests : RequestHandlerTestsBase<GetAvailableWebinarByIdQueryHandler>
{
    protected override GetAvailableWebinarByIdQueryHandler Handler { get; }
    public GetAvailableWebinarByIdQueryHandlerTests()
    {
        Handler = new (UnitOfWorkMock.Object);
    }

    [Theory]
    [AutoData]
    public async Task Handle_ReturnsNotFoundResult_PassedThroughRepository(AvailableWebinarByIdRequest request)
    {
        // Arrange
        WebinarRepositoryMock.Setup(x => x.GetAsync(
            It.IsAny<Expression<Func<Webinar, bool>>>(),
            It.IsAny<Func<IQueryable<Webinar>, IQueryable<Webinar>>?>(),
            true
        )).ReturnsAsync(new List<Webinar>());
        
        // Act
        var result = await Handler.Handle(request, CancellationToken.None);

        // Assert
        WebinarRepositoryMock.Verify(x => x.GetAsync(
            It.IsAny<Expression<Func<Webinar, bool>>>(),
            It.IsAny<Func<IQueryable<Webinar>, IQueryable<Webinar>>?>(),
            true
        ), Times.Once);
        result.Should().BeOfType<NotFoundObjectResult>();
    }
    
    [Theory]
    [AutoData]
    public async Task Handle_ReturnsBadRequestResult_PassedThroughRepository(AvailableWebinarByIdRequest request)
    {
        // Arrange
        WebinarRepositoryMock.Setup(x => x.GetAsync(
            It.IsAny<Expression<Func<Webinar, bool>>>(),
            It.IsAny<Func<IQueryable<Webinar>, IQueryable<Webinar>>?>(),
            true
        )).ReturnsAsync(new List<Webinar> {new Webinar {ScheduleDate = DateTime.UtcNow.AddDays(-4)}});
        
        // Act
        var result = await Handler.Handle(request, CancellationToken.None);

        // Assert
        WebinarRepositoryMock.Verify(x => x.GetAsync(
            It.IsAny<Expression<Func<Webinar, bool>>>(),
            It.IsAny<Func<IQueryable<Webinar>, IQueryable<Webinar>>?>(),
            true
        ), Times.Once);
        result.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [Theory]
    [AutoData]
    public async Task Handle_ReturnsOKResult_PassedThroughRepository(AvailableWebinarByIdRequest request)
    {
        // Arrange
        WebinarRepositoryMock.Setup(x => x.GetAsync(
            It.IsAny<Expression<Func<Webinar, bool>>>(),
            It.IsAny<Func<IQueryable<Webinar>, IQueryable<Webinar>>?>(),
            true
        )).ReturnsAsync(new List<Webinar> {new Webinar {ScheduleDate = DateTime.UtcNow}});
        
        // Act
        var result = await Handler.Handle(request, CancellationToken.None);

        // Assert
        WebinarRepositoryMock.Verify(x => x.GetAsync(
            It.IsAny<Expression<Func<Webinar, bool>>>(),
            It.IsAny<Func<IQueryable<Webinar>, IQueryable<Webinar>>?>(),
            true
        ), Times.Once);
        result.Should().BeOfType<OkObjectResult>();
    }
}