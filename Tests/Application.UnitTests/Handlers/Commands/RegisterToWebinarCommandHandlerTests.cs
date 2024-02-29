using System.Linq.Expressions;
using Application.Handlers.Commands;
using Application.Requests;
using AutoFixture;
using AutoFixture.Xunit2;
using Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace Application.UnitTests.Handlers.Commands;

public class RegisterToWebinarCommandHandlerTests : RequestHandlerTestsBase<RegisterToWebinarCommandHandler>
{
    protected override RegisterToWebinarCommandHandler Handler { get; }
    
    public RegisterToWebinarCommandHandlerTests()
    {
        Handler = Fixture.Create<RegisterToWebinarCommandHandler>();
    }

    [Theory]
    [AutoData]
    public async Task Handle_ReturnsNotFoundResult(RegisterWebinarRequest request)
    {
        // Arrange
        WebinarRepositoryMock.Setup(x => x.GetByIdAsync(request.WebinarId))
            .ReturnsAsync(default(Webinar));
        
        // Act
        var result = await Handler.Handle(request, CancellationToken.None);
        
        // Assert
        result.Should().BeOfType<NotFound<string>>();
    }
    
    [Theory]
    [AutoData]
    public async Task Handle_ReturnsNoContentResult(RegisterWebinarRequest request)
    {
        // Arrange
        WebinarRepositoryMock.Setup(x => x.GetByIdAsync(
             request.WebinarId)).ReturnsAsync(new Webinar{Id = request.WebinarId});
        
        // Act
        var result = await Handler.Handle(request, CancellationToken.None);
        
        // Assert
        result.Should().BeOfType<NoContent>();
    }
}