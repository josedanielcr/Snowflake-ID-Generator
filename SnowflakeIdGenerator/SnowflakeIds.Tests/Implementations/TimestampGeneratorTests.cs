using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using SnowflakeIds.Abstractions;
using SnowflakeIds.Common;
using SnowflakeIds.Implementations;

namespace SnowflakeIds.Tests.Implementations;

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
        var settings = new Settings
        {
            StartYear = startYear,
            StartMonth = startMonth,
            StartDay = startDay,
            TimestampLength = timestampLength
        };
        var optionsMock = new Mock<IOptions<Settings>>();
        optionsMock.Setup(o => o.Value).Returns(settings);
        
        var timestampGenerator = new TimestampGenerator(optionsMock.Object);
        
        //Act
        var timestamp = timestampGenerator.Generate();
        
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
        var settings = new Settings
        {
            StartYear = startYear,
            StartMonth = startMonth,
            StartDay = startDay,
            TimestampLength = timestampLength
        };
        var optionsMock = new Mock<IOptions<Settings>>();
        optionsMock.Setup(o => o.Value).Returns(settings);
        
        var timestampGenerator = new TimestampGenerator(optionsMock.Object);
    
        //Act
        var t1 = timestampGenerator.Generate();
        await Task.Delay(1); 
        var t2 = timestampGenerator.Generate();
        
        //Assert
        t2.Should().BeGreaterThan(t1);
    }
}