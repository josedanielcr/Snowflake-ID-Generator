using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using SnowflakeIds.Abstractions;
using SnowflakeIds.Common;
using SnowflakeIds.Implementations;
using SnowflakeIds.Tests.Shared.Fixtures;

namespace SnowflakeIds.Tests.Implementations;

public class TimestampGeneratorTests(SnowflakeSutFixture fx) : IClassFixture<SnowflakeSutFixture> 
{
    [Fact]
    public void GivenValidOptions_WhenGeneratingTimestamp_ThenReturnTimestamp()
    {
        //Arrange
        var (_, timestampManager, _, _) = fx.CreateAll();
        
        //Act
        var timestamp = timestampManager.Generate();
        
        //Assert
        timestamp.Should().BeGreaterThanOrEqualTo(0);
    }
    
    [Fact]
    public async Task GivenValidOptions_WhenGeneratingMultipleTimestamps_ThenTheyIncrease()
    {
        //Arrange
        var (_, timestampManager, _, _) = fx.CreateAll();
    
        //Act
        var t1 = timestampManager.Generate();
        await Task.Delay(1); 
        var t2 = timestampManager.Generate();
        
        //Assert
        t2.Should().BeGreaterThan(t1);
    }
}