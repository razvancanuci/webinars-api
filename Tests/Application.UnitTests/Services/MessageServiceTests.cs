using Application.Services.Implementations;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Messages;
using MassTransit;
using Moq;

namespace Application.UnitTests.Services;

public class MessageServiceTests
{
    private readonly Mock<IPublishEndpoint> _publisherMock;

    private readonly IMessageService _sut;
    public MessageServiceTests()
    {
        _publisherMock = new Mock<IPublishEndpoint>();
        _sut = new MessageService(_publisherMock.Object);
    }

    [Fact]
    public async Task Send_PassedThroughPublishEndpoint()
    {
        // Act
        await _sut.Send(new EmailCancellationMessage(new List<Person>()));
        
        // Assert
        _publisherMock.Verify(m => m.Publish(It.IsAny<EmailCancellationMessage>(), It.IsAny<CancellationToken>()));
    }
}