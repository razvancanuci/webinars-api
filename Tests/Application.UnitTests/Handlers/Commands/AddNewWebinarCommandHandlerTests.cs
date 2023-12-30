using System.Linq.Expressions;
using Application.Handlers.Commands;
using Application.Requests;
using AutoFixture.Xunit2;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Application.UnitTests.Handlers.Commands;

public class AddNewWebinarCommandHandlerTests : RequestHandlerTestsBase<AddNewWebinarCommandHandler>
{
    private readonly Mock<IFileStorage> _fileStorageMock;
    protected override AddNewWebinarCommandHandler Handler { get; }
    public AddNewWebinarCommandHandlerTests()
    {
        _fileStorageMock = new();
        Handler = new AddNewWebinarCommandHandler(UnitOfWorkMock.Object, _fileStorageMock.Object);
    }

    [Theory]
    [AutoData]
    public async Task Handle_ReturnsCreatedResult_PassedThroughRepositoryAndUnitOfWork(string title)
    {
        // Arrange
        var request = new NewWebinarRequest { Title = title };
        WebinarRepositoryMock.Setup(w => w.GetAsync(
            It.IsAny<Expression<Func<Webinar, bool>>>(),
            It.IsAny<Func<IQueryable<Webinar>, IQueryable<Webinar>>>(),
            It.IsAny<bool>()
        )).ReturnsAsync(new List<Webinar>{ new Webinar()});
        
        // Act
        var result = await Handler.Handle(request, CancellationToken.None);
        
        // Assert
        UnitOfWorkMock.Verify(x => x.SaveAsync(), Times.Once);
        WebinarRepositoryMock.Verify(x => x.InsertAsync(It.IsAny<Webinar>()), Times.Once);
        result.Should().BeOfType<CreatedResult>();
    }


}