using Microsoft.Extensions.Options;
using SnowflakeIds.Common;
using SnowflakeIds.Implementations;

namespace SnowflakeIds.Tests.Shared.Fixtures;

public class SnowflakeSutFixture : SnowflakeSettingsFixture
{
    public TimestampGenerator CreateTimestampGenerator(IOptions<Settings>? opts = null)
        => new(opts ?? CreateOptions());

    public SequenceManager CreateSequenceManager(IOptions<Settings>? opts = null,
        TimestampGenerator? tg = null)
    {
        var o = opts ?? CreateOptions();
        var t = tg ?? new TimestampGenerator(o);
        return new SequenceManager(o, t);
    }

    public SnowflakeComposer CreateSnowflakeComposer(IOptions<Settings>? opts = null)
        => new(opts ?? CreateOptions());
    
    public (IOptions<Settings> options,
        TimestampGenerator timestamp,
        SequenceManager sequence,
        SnowflakeComposer composer)
        CreateAll(Settings? overrides = null)
    {
        var opts = CreateOptions(overrides);
        var tg = new TimestampGenerator(opts);
        var sm = new SequenceManager(opts, tg);
        var comp = new SnowflakeComposer(opts);
        return (opts, tg, sm, comp);
    }
}