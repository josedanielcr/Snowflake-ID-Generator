namespace SnowflakeIds.Abstractions;

public interface ISequenceManager
{
    int GetSequence(ref long timestamp);
    int Next(long currentTimestamp);
}