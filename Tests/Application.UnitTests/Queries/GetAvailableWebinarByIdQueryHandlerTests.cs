using System.Linq.Expressions;
using Application.Queries;
using Application.Requests;
using AutoFixture.Xunit2;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Application.UnitTests.Queries;

public class GetAvailableWebinarByIdQueryHandlerTests
{
    private readonly Mock<IRepository<Webinar>> _webinarRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    
    private readonly GetAvailableWebinarByIdQueryHandler _handler;
    
    public GetAvailableWebinarByIdQueryHandlerTests()
    {
        _webinarRepositoryMock = new();
        
        _unitOfWorkMock = new();
        _unitOfWorkMock.Setup(x => x.WebinarRepository).Returns(_webinarRepositoryMock.Object);

        _handler = new (_unitOfWorkMock.Object);
    }

    [Theory]
    [AutoData]
    public async Task Handle_ReturnsNotFoundResult_PassedThroughRepository(AvailableWebinarByIdRequest request)
    {
        // Arrange
        _webinarRepositoryMock.Setup(x => x.GetAsync(
            It.IsAny<Expression<Func<Webinar, bool>>>(),
            It.IsAny<Func<IQueryable<Webinar>, IQueryable<Webinar>>?>(),
            true
        )).ReturnsAsync(new List<Webinar>());
        
        // Act
        var result = await _handler.Handle(request, new CancellationToken());

        // Assert
        _webinarRepositoryMock.Verify(x => x.GetAsync(
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
        _webinarRepositoryMock.Setup(x => x.GetAsync(
            It.IsAny<Expression<Func<Webinar, bool>>>(),
            It.IsAny<Func<IQueryable<Webinar>, IQueryable<Webinar>>?>(),
            true
        )).ReturnsAsync(new List<Webinar> {new Webinar {ScheduleDate = DateTime.UtcNow.AddDays(-4)}});
        
        // Act
        var result = await _handler.Handle(request, new CancellationToken());

        // Assert
        _webinarRepositoryMock.Verify(x => x.GetAsync(
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
        _webinarRepositoryMock.Setup(x => x.GetAsync(
            It.IsAny<Expression<Func<Webinar, bool>>>(),
            It.IsAny<Func<IQueryable<Webinar>, IQueryable<Webinar>>?>(),
            true
        )).ReturnsAsync(new List<Webinar> {new Webinar {ScheduleDate = DateTime.UtcNow}});
        
        // Act
        var result = await _handler.Handle(request, new CancellationToken());

        // Assert
        _webinarRepositoryMock.Verify(x => x.GetAsync(
            It.IsAny<Expression<Func<Webinar, bool>>>(),
            It.IsAny<Func<IQueryable<Webinar>, IQueryable<Webinar>>?>(),
            true
        ), Times.Once);
        result.Should().BeOfType<OkObjectResult>();
    }
}