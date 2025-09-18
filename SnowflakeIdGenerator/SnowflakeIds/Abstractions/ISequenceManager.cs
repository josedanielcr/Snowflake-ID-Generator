namespace SnowflakeIds.Abstractions;

public interface ISequenceManager
{
    int Next(long currentTimestamp);
}