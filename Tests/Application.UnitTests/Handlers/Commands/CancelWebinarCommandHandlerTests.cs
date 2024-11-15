﻿using Application.Handlers.Commands;
using Application.Requests;
using Application.Services.Interfaces;
using AutoFixture;
using AutoFixture.Xunit2;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Messages;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace Application.UnitTests.Handlers.Commands;

public class CancelWebinarCommandHandlerTests : RequestHandlerTestsBase<CancelWebinarCommandHandler>
{
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly Mock<IMessageService> _messageServiceMock;
    protected override CancelWebinarCommandHandler Handler { get; }
    
    public CancelWebinarCommandHandlerTests()
    {
        _cacheServiceMock = Fixture.Freeze<Mock<ICacheService>>();
        _messageServiceMock = Fixture.Freeze<Mock<IMessageService>>();
        
        Handler = Fixture.Create<CancelWebinarCommandHandler>();
    }

    [Theory]
    [AutoData]
    public async Task Handle_Returns_NotFoundResult(CancelWebinarRequest request)
    {
        // Arrange
        WebinarRepositoryMock.Setup(x => x.GetByIdAsync(
            request.WebinarId)).ReturnsAsync(default(Webinar));
        
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
        WebinarRepositoryMock.Setup(x => x.GetByIdAsync(
            request.WebinarId)).ReturnsAsync( new Webinar { Id = request.WebinarId });
        
        // Act
        var result = await Handler.Handle(request, CancellationToken.None);
        
        // Assert
        _cacheServiceMock.Verify(m => m.DeleteKeyAsync(request.KeyToDelete, It.IsAny<CancellationToken>()));
        result.Should().BeOfType<NoContent>();
    }
    
    [Theory]
    [AutoData]
    public async Task Handle_Returns_NoContentResult_PassedThroughMessageService(CancelWebinarRequest request)
    {
        // Arrange
        WebinarRepositoryMock.Setup(x => x.GetByIdAsync(
            request.WebinarId)).ReturnsAsync( new Webinar
        {
            Id = request.WebinarId, PeopleRegistered = new List<Person>{new Person()}
        });
        
        // Act
        var result = await Handler.Handle(request, CancellationToken.None);
        
        // Assert
        _messageServiceMock.Verify(m => m.Send(It.IsAny<EmailCancellationMessage>(), It.IsAny<CancellationToken>()));
        result.Should().BeOfType<NoContent>();
    }
}