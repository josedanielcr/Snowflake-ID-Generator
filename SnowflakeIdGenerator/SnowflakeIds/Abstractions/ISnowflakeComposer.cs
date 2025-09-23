namespace SnowflakeIds.Abstractions;

public interface ISnowflakeComposer
{
    long Compose(long timestamp, int sequence);
}