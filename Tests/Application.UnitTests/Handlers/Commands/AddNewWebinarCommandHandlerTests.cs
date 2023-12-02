using Application.Handlers.Commands;
using Application.Requests;
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
        Handler = new AddNewWebinarCommandHandler(UnitOfWorkMock.Object);
    }

    [Theory]
    [AutoData]
    public async Task Handle_ReturnsCreatedResult_PassedThroughRepositoryAndUnitOfWork(NewWebinarRequest request)
    {
        // Act
        var result = await Handler.Handle(request, CancellationToken.None);
        
        // Assert
        UnitOfWorkMock.Verify(x => x.SaveAsync(), Times.Once);
        WebinarRepositoryMock.Verify(x => x.InsertAsync(It.IsAny<Webinar>()), Times.Once);
        result.Should().BeOfType<CreatedResult>();
    }


}