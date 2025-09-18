using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SnowflakeIds.Abstractions;
using SnowflakeIds.Common;
using SnowflakeIds.Helpers;

namespace SnowflakeIds.Implementations;

public class IdGenerator(IOptions<Settings> options) : IIdGenerator
{
    public long Next()
    {
        var timestamp = TimestampGenerator.Generate(
            options.Value.StartYear,
            options.Value.StartMonth,
            options.Value.StartDay,
            options.Value.TimestampLength);
        
        WorkerValidator.Validate(options.Value.WorkerId, options.Value.WorkerLength);
        return 1L;
    }
}