using Application.Services.Implementations;
using Application.Services.Interfaces;
using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using StackExchange.Redis;

namespace Application.UnitTests.Services;

public class CacheServiceTests
{
    private readonly Mock<IConnectionMultiplexer> _connectionMultiplexerMock;
    private readonly Mock<IDatabase> _databaseMock;

    private readonly ICacheService _sut;

    public CacheServiceTests()
    {
        _connectionMultiplexerMock = new Mock<IConnectionMultiplexer>();
        _databaseMock = new Mock<IDatabase>();

        _connectionMultiplexerMock.Setup(m => m.GetDatabase(
                It.IsAny<int>(),
                It.IsAny<object>()))
            .Returns(_databaseMock.Object);

        _sut = new CacheService(_connectionMultiplexerMock.Object);
    }

    [Theory]
    [AutoData]
    public async Task GetOrCreate_Returns_TheCacheResult_PassedThroughCacheMockGetMethod(string key)
    {
        // Arrange
        _databaseMock.Setup(x => x.StringGetAsync(key, It.IsAny<CommandFlags>()))
            .ReturnsAsync(new RedisValue("[1, 2, 3]"));

        // Act
        var result = await _sut.GetOrCreateAsync(key,
            () => Task.FromResult(new List<int>()));

        // Assert
        result.Should().NotBeEmpty();
        result.Should().HaveCount(3);
        _databaseMock.Verify(x => x.StringGetAsync(key, It.IsAny<CommandFlags>()));
    }

    [Theory]
    [AutoData]
    public async Task GetOrCreate_Returns_QuryResult_PassedThroughCacheMockSetMethod(string key)
    {
        // Act
        var result = await _sut.GetOrCreateAsync(key,
            () => Task.FromResult(new List<int>()));

        // Assert
        result.Should().BeEmpty();
        _databaseMock.Verify(x => x.StringSetAsync(key,
            It.IsAny<RedisValue>(),
            It.IsAny<TimeSpan>(),
            It.IsAny<bool>(),
            It.IsAny<When>(),
            It.IsAny<CommandFlags>()));
    }
}