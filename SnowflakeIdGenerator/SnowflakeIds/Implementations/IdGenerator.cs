using Microsoft.Extensions.Options;
using SnowflakeIds.Abstractions;
using SnowflakeIds.Common;
using SnowflakeIds.Helpers;

namespace SnowflakeIds.Implementations;

public class IdGenerator(IOptions<Settings> options, ISequenceManager sequenceManager, 
    ITimestampGenerator timestampGenerator, ISnowflakeComposer snowflakeComposer) : IIdGenerator
{
    public long Next()
    {
        var timestamp = timestampGenerator.Generate();
        WorkerValidator.Validate(options.Value.WorkerId, options.Value.WorkerLength);
        var sequence = sequenceManager.GetSequence(ref timestamp);
        return snowflakeComposer.Compose(timestamp, sequence);
    }
}