using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using SnowflakeIds.Abstractions;
using SnowflakeIds.Common;
using SnowflakeIds.Implementations;

namespace SnowflakeIds.Extensions;

public static class ServiceCollectionExtensions
{
    public static OptionsBuilder<Settings> AddSnowflakeIds(this IServiceCollection services, Action<Settings> configure)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configure);

        return services.AddSnowflakeIds().Configure(configure);
    }
    
    public static OptionsBuilder<Settings> AddSnowflakeIds(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.TryAddSingleton<ITimestampGenerator, TimestampGenerator>();
        services.TryAddSingleton<ISequenceManager, SequenceManager>();
        services.TryAddSingleton<ISnowflakeComposer, SnowflakeComposer>();
        services.TryAddSingleton<IIdGenerator, IdGenerator>();

        return services.AddOptions<Settings>();
    }
}