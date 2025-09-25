using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using SnowflakeIds.Common;
using SnowflakeIds.Implementations;
using SnowflakeIds.Tests.Shared.Fixtures;

namespace SnowflakeIds.Tests.Implementations;

public class SnowflakeComposerTests(SnowflakeSutFixture fx) : IClassFixture<SnowflakeSutFixture>
{
    [Fact]
    public void GivenValidTimestampAndSequence_WhenComposingSnowflakeId_ThenValidIdIsReturned()
    {
        //Arrange
        var (_, timestampGenerator, sequenceManager, snowflakeComposer) = fx.CreateAll();
        var timestamp = timestampGenerator.Generate();
        var sequence = sequenceManager.GetSequence(ref timestamp);
        
        //Act
        var id = snowflakeComposer.Compose(timestamp, sequence);
        
        //Assert
        id.Should().NotBe(null);
    }
}