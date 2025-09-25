using Microsoft.Extensions.Options;
using Moq;
using SnowflakeIds.Common;

namespace SnowflakeIds.Tests.Shared.Fixtures;

public class SnowflakeSettingsFixture
{
    private Settings DefaultSettings => new()
    {
        StartYear = 2025,
        StartMonth = 1,
        StartDay = 1,
        TimestampLength = 41,
        SequenceLength = 12,
        WorkerLength = 10,
        WorkerId = 1
    };
    
    public IOptions<Settings> CreateOptions(Settings? overrides = null)
    {
        var settings = overrides ?? DefaultSettings;
        var mock = new Mock<IOptions<Settings>>();
        mock.Setup(o => o.Value).Returns(settings);
        return mock.Object;
    }
}