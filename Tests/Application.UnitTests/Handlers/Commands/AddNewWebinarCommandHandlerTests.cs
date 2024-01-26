using Application.Handlers.Commands;
using Application.Requests;
using AutoFixture;
using AutoFixture.Xunit2;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Application.UnitTests.Handlers.Commands;

public class AddNewWebinarCommandHandlerTests : RequestHandlerTestsBase<AddNewWebinarCommandHandler>
{
    protected override AddNewWebinarCommandHandler Handler { get; }
    public AddNewWebinarCommandHandlerTests()
    {
        Handler = Fixture.Create<AddNewWebinarCommandHandler>();
    }

    [Theory]
    [AutoData]
    public async Task Handle_ReturnsCreatedResult_PassedThroughRepositoryAndUnitOfWork(string title)
    {
        // Arrange
        var request = new NewWebinarRequest { Title = title };
        
        // Act
        var result = await Handler.Handle(request, CancellationToken.None);
        
        // Assert
        UnitOfWorkMock.Verify(x => x.SaveAsync(), Times.Once);
        WebinarRepositoryMock.Verify(x => x.InsertAsync(It.IsAny<Webinar>()), Times.Once);
        result.Should().BeOfType<CreatedResult>();
    }


}