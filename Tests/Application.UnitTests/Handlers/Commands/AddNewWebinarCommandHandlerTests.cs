using Application.Handlers.Commands;
using Application.Requests;
using AutoFixture;
using AutoFixture.Xunit2;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace Application.UnitTests.Handlers.Commands;

public class AddNewWebinarCommandHandlerTests : RequestHandlerTestsBase<AddNewWebinarCommandHandler>
{
    protected override AddNewWebinarCommandHandler Handler { get; }

    private readonly Mock<IContentModerationService> _contentModerationServiceMock;
    public AddNewWebinarCommandHandlerTests()
    {
        _contentModerationServiceMock = Fixture.Freeze<Mock<IContentModerationService>>();
        
        Handler = Fixture.Create<AddNewWebinarCommandHandler>();
    }

    [Theory]
    [AutoData]
    public async Task Handle_ReturnsCreatedResult_PassedThroughRepositoryAndUnitOfWork(string title)
    {
        // Arrange
        var request = new NewWebinarRequest { Title = title, Image = new FormFile(new MemoryStream(1), 2,1,"a","a.png") };
        
        // Act
        var result = await Handler.Handle(request, CancellationToken.None);
        
        // Assert
        UnitOfWorkMock.Verify(x => x.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        WebinarRepositoryMock.Verify(x => x.InsertAsync(It.IsAny<Webinar>()), Times.Once);
        result.Should().BeOfType<Created<NewWebinarRequest>>();
    }
    
    [Fact]
    public async Task Handle_ContentModeratorReturnsTrue_ReturnsBadRequestResult()
    {
        // Arrange
        var request = new NewWebinarRequest { Title = "t", Image = new FormFile(new MemoryStream(1), 2,1,"a","a.png") };

        _contentModerationServiceMock.Setup(m => m.IsRacyOrAdultImage(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        
        // Act
        var result = await Handler.Handle(request, CancellationToken.None);
        
        // Assert
        result.Should().BeOfType<BadRequest<string>>();
    }


}