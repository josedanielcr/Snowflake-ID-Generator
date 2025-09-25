using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SnowflakeIds.Abstractions;
using SnowflakeIds.Common;
using SnowflakeIds.Extensions;

namespace SnowflakeIds.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void GivenValidConfiguration_WhenAddingSnowflakeIdsDependency_ThenShouldRegistersGeneratorDependencies()
    {
        //Assert
        var services = new ServiceCollection();
        services.AddSnowflakeIds(options =>
        {
            options.StartYear = 2025;
            options.StartMonth = 1;
            options.StartDay = 1;
            options.TimestampLength = 41;
            options.SequenceLength = 12;
            options.WorkerLength = 10;
            options.WorkerId = 1;
        });
        
        //Act
        using var provider = services.BuildServiceProvider();
        var generator = provider.GetRequiredService<IIdGenerator>();
        var settings = provider.GetRequiredService<IOptions<Settings>>().Value;
        
        //Assert
        generator.Next().Should().BeGreaterThan(0);
        settings.WorkerId.Should().Be(1);
        settings.TimestampLength.Should().Be(41);
    }

    [Fact]
    public void GivenNoConfiguration_WhenAddingSnowflakeIdsDependency_ThenShouldAllowsManualOptionsConfiguration()
    {
        //Arrange
        var services = new ServiceCollection();
        services.AddSnowflakeIds();
        services.PostConfigure<Settings>(options =>
        {
            options.StartYear = 2025;
            options.StartMonth = 1;
            options.StartDay = 1;
            options.TimestampLength = 41;
            options.SequenceLength = 12;
            options.WorkerLength = 10;
            options.WorkerId = 1;
        });
        
        // Act
        using var provider = services.BuildServiceProvider();
        var generator = provider.GetRequiredService<IIdGenerator>();
        
        // Assert
        generator.Next().Should().BeGreaterThan(0);
    }
}