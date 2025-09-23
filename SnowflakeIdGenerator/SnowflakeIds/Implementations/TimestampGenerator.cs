using Microsoft.Extensions.Options;
using SnowflakeIds.Abstractions;
using SnowflakeIds.Common;

namespace SnowflakeIds.Implementations;

public class TimestampGenerator(IOptions<Settings> options) : ITimestampGenerator
{
    private readonly int _startYear = options.Value.StartYear;
    private readonly int _startMonth = options.Value.StartMonth;
    private readonly int _startDay = options.Value.StartDay;
    private readonly int _timestampLength = options.Value.TimestampLength;

    public long Generate()
    {
        var epoch = new DateTime(_startYear, _startMonth, _startDay, 0, 0, 0, DateTimeKind.Utc);
        var mask =  (1L << _timestampLength) - 1;
        
        var nowUtc = DateTime.UtcNow;
        var elapsed = nowUtc - epoch;
        var ms = (long) elapsed.TotalMilliseconds;
        return ms & mask;
    }
}