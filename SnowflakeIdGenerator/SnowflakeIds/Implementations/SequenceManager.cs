using Microsoft.Extensions.Options;
using SnowflakeIds.Abstractions;
using SnowflakeIds.Common;

namespace SnowflakeIds.Implementations;

public class SequenceManager(IOptions<Settings> options, ITimestampGenerator timestampGenerator) : ISequenceManager
{
    private readonly Lock _lock = new();
    private long _lastTimestamp = -1;
    private int _seq = 0;
    
    public int GetSequence(ref long timestamp)
    {
        int sequence;
        while ((sequence = Next(timestamp)) == -1)
        {
            Thread.Sleep(1);
            timestamp = timestampGenerator.Generate();
        }
        return sequence;
    }
    
    public int Next(long currentTimestamp)
    {
        var maxSeq = (1 << options.Value.SequenceLength) - 1;
        lock(_lock)
        {
            if (currentTimestamp == _lastTimestamp)
            {
                if (_seq == maxSeq)
                {
                    return -1;
                }
                return ++_seq;
            }
            _lastTimestamp = currentTimestamp;
            _seq = 0;
            return _seq;
        }
    }
}