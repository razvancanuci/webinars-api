using System.Text;
using Application.Services.Implementations;
using Application.Services.Interfaces;
using AutoFixture.Xunit2;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using StackExchange.Redis;

namespace Application.UnitTests.Services;

public class CacheServiceTests
{
    private readonly Mock<IDistributedCache> _distributedCacheMock;

    private readonly ICacheService _sut;

    public CacheServiceTests()
    {
        _distributedCacheMock = new();
        
        _sut = new CacheService(_distributedCacheMock.Object);
    }

    [Theory]
    [AutoData]
    public async Task GetOrCreate_Returns_TheCacheResult_PassedThroughCacheMockGetMethod(string key)
    {
        // Arrange
        _distributedCacheMock.Setup(x => x.GetAsync(key, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Encoding.ASCII.GetBytes("[1, 2, 3]"));

        // Act
        var result = await _sut.GetOrCreateAsync(key,
            () => ValueTask.FromResult(new List<int>()));

        // Assert
        result.Should().NotBeEmpty();
        result.Should().HaveCount(3);
        _distributedCacheMock.Verify(x => x.GetAsync(key, It.IsAny<CancellationToken>()));
    }

    [Theory]
    [AutoData]
    public async Task GetOrCreate_Returns_QueryResult_PassedThroughCacheMockSetMethod(string key)
    {
        // Act
        var result = await _sut.GetOrCreateAsync(key,
            () => ValueTask.FromResult(new List<int>()));

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeEmpty();
            _distributedCacheMock.Verify(x => x.SetAsync(key,
                It.IsAny<byte[]>(),
                It.IsAny<DistributedCacheEntryOptions>(),
                It.IsAny<CancellationToken>()));
        }
    }
}