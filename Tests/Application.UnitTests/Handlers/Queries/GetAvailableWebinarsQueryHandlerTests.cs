using Application.Handlers.Queries;
using Application.Requests;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace Application.UnitTests.Handlers.Queries;

public class GetAvailableWebinarsQueryHandlerTests : RequestHandlerTestsBase<GetAvailableWebinarsQueryHandler>
{
    protected override GetAvailableWebinarsQueryHandler Handler { get; }

    public GetAvailableWebinarsQueryHandlerTests()
    {
        Handler = new GetAvailableWebinarsQueryHandler(UnitOfWorkMock.Object);
    }

    [Theory]
    [AutoData]
    public async Task Handle_ReturnsOKResult(AvailableWebinarsRequest request)
    {
        // Act
        var result = await Handler.Handle(request, CancellationToken.None);
        
        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }
}