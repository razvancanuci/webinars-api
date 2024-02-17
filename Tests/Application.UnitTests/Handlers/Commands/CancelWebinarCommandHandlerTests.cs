using Application.Handlers.Commands;
using Application.Requests;
using Application.Services.Interfaces;
using AutoFixture;
using AutoFixture.Xunit2;
using Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace Application.UnitTests.Handlers.Commands;

public class CancelWebinarCommandHandlerTests : RequestHandlerTestsBase<CancelWebinarCommandHandler>
{
    private readonly Mock<ICacheService> _cacheServiceMock;
    protected override CancelWebinarCommandHandler Handler { get; }
    
    public CancelWebinarCommandHandlerTests()
    {
        _cacheServiceMock = Fixture.Freeze<Mock<ICacheService>>();
        Handler = Fixture.Create<CancelWebinarCommandHandler>();
    }

    [Theory]
    [AutoData]
    public async Task Handle_Returns_NotFoundResult(CancelWebinarRequest request)
    {
        // Act
        var result = await Handler.Handle(request, CancellationToken.None);
        
        // Assert
        result.Should().BeOfType<NotFound<string>>();
    }
    
    [Theory]
    [AutoData]
    public async Task Handle_Returns_NoContentResult_PassedThroughCacheDelete(CancelWebinarRequest request)
    {
        // Arrange
        WebinarRepositoryMock.Setup(x => x.GetAsync(
            w => w.Id == request.WebinarId,
            null,
            false
        )).ReturnsAsync(new List<Webinar> { new Webinar { Id = request.WebinarId }});
        
        // Act
        var result = await Handler.Handle(request, CancellationToken.None);
        
        // Assert
        _cacheServiceMock.Verify(m => m.DeleteKeyAsync(request.KeyToDelete));
        result.Should().BeOfType<NoContent>();
    }
}