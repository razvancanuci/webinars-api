using Application.Handlers.Queries;
using Application.Requests;
using AutoFixture;
using Domain.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace Application.UnitTests.Handlers.Queries;

public class DownloadWebinarImageQueryHandlerTests : RequestHandlerTestsBase<DownloadWebinarImageQueryHandler>
{
    private readonly Mock<IFileStorage> _fileStorageMock;
    
    protected override DownloadWebinarImageQueryHandler Handler { get; }
    
    public DownloadWebinarImageQueryHandlerTests()
    {
        _fileStorageMock = Fixture.Freeze<Mock<IFileStorage>>();

        Handler = Fixture.Create<DownloadWebinarImageQueryHandler>();
    }

    [Fact]
    public async Task Handle_DefaultResponseOnBlobReturned_ReturnsNotFound()
    {
        // Arrange
        _fileStorageMock.Setup(x => x.GetAsync(
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(default((Stream, string)));
        // Act
        var result = await Handler.Handle(new DownloadWebinarImageRequest(string.Empty), CancellationToken.None);
        
        // Assert
        result.Should().BeOfType<NotFound<string>>();
    }
    
    [Fact]
    public async Task Handle_ImageReturned_ReturnsNotFound()
    {
        // Act
        var result = await Handler.Handle(new DownloadWebinarImageRequest(string.Empty), CancellationToken.None);
        
        // Assert
        result.Should().BeOfType<FileStreamHttpResult>();
    }
}