namespace SnowflakeIds.Helpers;

public static class TimestampGenerator
{
    public static long Generate(int startYear, int startMonth, int startDay, int timestampLength)
    {
        var epoch = new DateTime(startYear, startMonth, startDay, 0, 0, 0, DateTimeKind.Utc);
        var mask =  (1L << timestampLength) - 1;
        
        var nowUtc = DateTime.UtcNow;
        var elapsed = nowUtc - epoch;
        var ms = (long) elapsed.TotalMilliseconds;
        return ms & mask;
    }
}