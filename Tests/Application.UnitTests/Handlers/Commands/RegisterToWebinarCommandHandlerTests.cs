using System.Linq.Expressions;
using Application.Handlers.Commands;
using Application.Requests;
using AutoFixture.Xunit2;
using Domain.Entities;
using FluentAssertions;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Application.UnitTests.Handlers.Commands;

public class RegisterToWebinarCommandHandlerTests : RequestHandlerTestsBase<RegisterToWebinarCommandHandler>
{
    private readonly Mock<IPublishEndpoint> _publishEndpointMock;
    protected override RegisterToWebinarCommandHandler Handler { get; }
    
    public RegisterToWebinarCommandHandlerTests()
    {
        _publishEndpointMock = new();
        Handler = new RegisterToWebinarCommandHandler(_publishEndpointMock.Object, UnitOfWorkMock.Object);
    }

    [Theory]
    [AutoData]
    public async Task Handle_ReturnsNotFoundResult(RegisterWebinarRequest request)
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
        result.Should().BeOfType<NotFoundResult>();
    }
    
    [Theory]
    [AutoData]
    public async Task Handle_ReturnsNoContentResult(RegisterWebinarRequest request)
    {
        // Arrange
        WebinarRepositoryMock.Setup(x => x.GetAsync(
            entity => entity.Id == request.WebinarId,
            It.IsAny<Func<IQueryable<Webinar>, IQueryable<Webinar>>?>(),
            false
        )).ReturnsAsync(new List<Webinar>{new Webinar{Id = request.WebinarId}});
        
        // Act
        var result = await Handler.Handle(request, CancellationToken.None);
        
        // Assert
        result.Should().BeOfType<NoContentResult>();
    }
}