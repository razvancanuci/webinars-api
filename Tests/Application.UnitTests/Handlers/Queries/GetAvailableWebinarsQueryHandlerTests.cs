using Application.Handlers.Queries;
using Application.Requests;
using AutoFixture;
using AutoFixture.Xunit2;
using Domain.Dtos;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Application.UnitTests.Handlers.Queries;

public class GetAvailableWebinarsQueryHandlerTests : RequestHandlerTestsBase<GetAvailableWebinarsQueryHandler>
{
    protected override GetAvailableWebinarsQueryHandler Handler { get; }

    public GetAvailableWebinarsQueryHandlerTests()
    {
        Handler = Fixture.Create<GetAvailableWebinarsQueryHandler>();
    }

    [Theory]
    [AutoData]
    public async Task Handle_ReturnsOKResult(AvailableWebinarsRequest request)
    {
        // Act
        var result = await Handler.Handle(request, CancellationToken.None);
        
        // Assert
        result.Should().BeOfType<Ok<IEnumerable<WebinarShortInfoDto>>>();
    }
}