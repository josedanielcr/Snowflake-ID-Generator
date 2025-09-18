using System.Globalization;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using SnowflakeIds.Common;
using SnowflakeIds.Helpers;

namespace SnowflakeIds.Tests.Helpers;

public class TimestampGeneratorTests
{
    [Fact]
    public void GivenValidOptions_WhenGeneratingTimestamp_ThenReturnTimestamp()
    {
        //Arrange
        const int startYear = 2025;
        const int startMonth = 1;
        const int startDay = 1;
        const int timestampLength = 41;
        
        //Act
        var timestamp = TimestampGenerator.Generate(startYear, startMonth, startDay, timestampLength);
        
        //Assert
        timestamp.Should().BeGreaterThanOrEqualTo(0);
    }
    
    [Fact]
    public async Task GivenValidOptions_WhenGeneratingMultipleTimestamps_ThenTheyIncrease()
    {
        //Arrange
        const int startYear = 2025;
        const int startMonth = 1;
        const int startDay = 1;
        const int timestampLength = 41;
    
        //Act
        var t1 = TimestampGenerator.Generate(startYear, startMonth, startDay, timestampLength);
        await Task.Delay(1); 
        var t2 = TimestampGenerator.Generate(startYear, startMonth, startDay, timestampLength);
        
        //Assert
        t2.Should().BeGreaterThan(t1);
    }
}