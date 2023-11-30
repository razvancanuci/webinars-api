using Application.Commands;
using Application.Requests;
using AutoFixture.Xunit2;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Application.UnitTests.Commands;

public class AddNewWebinarCommandHandlerTests
{
    private readonly Mock<IRepository<Webinar>> _webinarRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    
    private readonly AddNewWebinarCommandHandler _handler;
    public AddNewWebinarCommandHandlerTests()
    {
        _webinarRepositoryMock = new();
        
        _unitOfWorkMock = new();
        _unitOfWorkMock.Setup(x => x.WebinarRepository).Returns(_webinarRepositoryMock.Object);
        
        _handler = new AddNewWebinarCommandHandler(_unitOfWorkMock.Object);
    }

    [Theory]
    [AutoData]
    public async Task Handle_ReturnsCreatedResult_PassedThroughRepositoryAndUnitOfWork(NewWebinarRequest request)
    {
        // Act
        var result = await _handler.Handle(request, new CancellationToken());
        
        // Assert
        _unitOfWorkMock.Verify(x => x.SaveAsync(), Times.Once);
        _webinarRepositoryMock.Verify(x => x.InsertAsync(It.IsAny<Webinar>()), Times.Once);
        result.Should().BeOfType<CreatedResult>();
    }
}