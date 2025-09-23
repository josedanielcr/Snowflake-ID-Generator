using Microsoft.Extensions.Options;
using SnowflakeIds.Abstractions;
using SnowflakeIds.Common;

namespace SnowflakeIds.Implementations;

public class SnowflakeComposer(IOptions<Settings> options) : ISnowflakeComposer
{
    private readonly int _sequenceLength = options.Value.SequenceLength;
    private readonly int _workerLength = options.Value.WorkerLength;
    private readonly int _timestampLength = options.Value.TimestampLength;
    private readonly int _workerId = options.Value.WorkerId;
    
    public long Compose(long timestamp, int sequence)
    {
        var workerShift = GetShifts(out var timestampShift);
        var tsMask = GetMasks(out var wrkMask, out var seqMask);
        return ComposeId(timestamp, sequence, tsMask, timestampShift, wrkMask, workerShift, seqMask);
    }

    private long ComposeId(long timestamp, int sequence, long tsMask, int timestampShift, long wrkMask, int workerShift,
        int seqMask)
    {
        var tsPart   = (timestamp & tsMask) << timestampShift;
        var workerPart = ((long)(_workerId & (int)wrkMask)) << workerShift;
        var seqPart  = (uint)(sequence & seqMask);

        return tsPart | workerPart | seqPart;
    }

    private long GetMasks(out long wrkMask, out int seqMask)
    {
        var tsMask  = (1L << _timestampLength) - 1;
        wrkMask = (1L << _workerLength) - 1;
        seqMask = (1  << _sequenceLength) - 1;
        return tsMask;
    }

    private int GetShifts(out int timestampShift)
    {
        var workerShift = _sequenceLength;
        timestampShift = _sequenceLength + _workerLength;
        return workerShift;
    }
}