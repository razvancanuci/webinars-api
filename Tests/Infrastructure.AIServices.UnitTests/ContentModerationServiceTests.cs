using AIServices;
using Domain.Interfaces;
using FluentAssertions;
using Microsoft.Azure.CognitiveServices.ContentModerator;
using Microsoft.Azure.CognitiveServices.ContentModerator.Models;
using Microsoft.Rest;
using Moq;

namespace Infrastructure.AIServices.UnitTests;

public class ContentModerationServiceTests
{
    private readonly Mock<IContentModeratorClient> _clientMock;

    private readonly IContentModerationService _sut;
    public ContentModerationServiceTests()
    {
        _clientMock = new();

        _sut = new ContentModerationService(_clientMock.Object);
    }

    [Fact]
    public async Task IsRacyOrAdultImage_PassedThroughClientImageModerationFalseResult()
    {
        // Arrange
        var stream = new MemoryStream();
        var imageModerationMock = new Mock<IImageModeration>();
        
        _clientMock
            .Setup(m => m.ImageModeration)
            .Returns(imageModerationMock.Object);

        imageModerationMock
            .Setup(m => m.EvaluateFileInputWithHttpMessagesAsync(stream,
                It.IsAny<bool?>(),
                It.IsAny<Dictionary<string, List<string>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new HttpOperationResponse<Evaluate>
            {
                Body = new Evaluate
                {
                    IsImageAdultClassified = false,
                    IsImageRacyClassified = false
                }
               
            });
        
        // Act
        var result = await _sut.IsRacyOrAdultImage(stream);

        // Assert
        _clientMock.Verify(x => x.ImageModeration);
        result.Should().BeFalse();
    }
}