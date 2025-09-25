using FluentAssertions;
using SnowflakeIds.Implementations;
using SnowflakeIds.Tests.Shared.Fixtures;

namespace SnowflakeIds.Tests.Implementations;

public class IdGeneratorTests(SnowflakeSutFixture fx) : IClassFixture<SnowflakeSutFixture>
{
    [Fact]
    public void GivenValidConfiguration_WhenGeneratingAndId_ThenReturnValidSnowflakeId()
    {
        //Arrange
        var (options, timestampManager, sequenceManager, composer) = fx.CreateAll();
        var idGenerator = new IdGenerator(options, sequenceManager, timestampManager, composer);

        //Act
        var snowflakeId = idGenerator.Next();

        //Assert
        snowflakeId.Should().BeGreaterThan(0);
        snowflakeId.Should().NotBe(null);
        snowflakeId.Should().NotBe(0);
    }

    [Fact]
    public void GivenValidConfiguration_WhenGeneratingTwoIdsAtTheSameMillisecond_ThenReturnValidAndDifferentSnowflakeIds()
    {
        //Assert
        var (options, timestampManager, sequenceManager, composer) = fx.CreateAll();
        var idGenerator = new IdGenerator(options, sequenceManager, timestampManager, composer);
        
        //Act
        var snowflakeId1 = idGenerator.Next();
        var snowflakeId2 = idGenerator.Next();
        
        //Assert
        snowflakeId1.Should().NotBe(snowflakeId2);
        snowflakeId2.Should().BeGreaterThan(snowflakeId1);
    }
}