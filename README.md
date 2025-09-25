# SnowflakeIds

A lightweight Snowflake-style ID generator for .NET 9+. It provides small, testable components and an IServiceCollection extension so you can inject a single IIdGenerator and start producing unique, ordered 64-bit IDs partitioned by worker.

> **Why Snowflake IDs?** Snowflake IDs combine a timestamp, worker identifier, and per-timestamp sequence into a compact 64-bit integer. They let you scale horizontally without coordinating a central database auto-increment and preserve chronological ordering.

## Features

- ✅ **Thread-safe orchestration** – `IdGenerator` coordinates timestamp, sequence, and composition services without leaking concurrency primitives to callers.
- ⚙️ **Configurable bit layout** – tune timestamp, worker, and sequence bit lengths plus the epoch start date to match your scale needs.
- 🧩 **Dependency-injection friendly** – call `services.AddSnowflakeIds(...)` and inject `IIdGenerator` anywhere in your application.
- 🧪 **Well-tested** – granular unit tests cover every component and the full generation pipeline.

## Getting started

### 1. Install the package

Use whichever tooling you prefer. All commands install version `1.0.0` from NuGet:

```bash
# .NET CLI
dotnet add package SnowflakeIds --version 1.0.0
```

```powershell
# Package Manager Console
Install-Package SnowflakeIds -Version 1.0.0
```

```xml
<!-- PackageReference -->
<ItemGroup>
  <PackageReference Include="SnowflakeIds" Version="1.0.0" />
</ItemGroup>
```

### 2. Register the generator (ASP.NET Core / worker services)

```csharp
using Microsoft.Extensions.DependencyInjection;
using SnowflakeIds.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSnowflakeIds(options =>
{
    options.StartYear = 2025;
    options.StartMonth = 1;
    options.StartDay = 1;
    options.TimestampLength = 41; // milliseconds (~69 years)
    options.WorkerLength = 10;    // up to 1024 workers
    options.SequenceLength = 12;  // up to 4096 IDs per millisecond
    options.WorkerId = 42;        // unique per node
});

var app = builder.Build();
// ...
app.Run();
```

Now any service can request `IIdGenerator`:

```csharp
public class OrderNumberFactory(IIdGenerator generator)
{
    public long CreateOrderId() => generator.Next();
}
```

### 3. Bind from configuration instead of code (optional)

If you keep Snowflake settings in `appsettings.json`, pair `AddSnowflakeIds()` with `Configure<Settings>`:

```json
// appsettings.json
{
  "Snowflake": {
    "StartYear": 2025,
    "StartMonth": 1,
    "StartDay": 1,
    "TimestampLength": 41,
    "WorkerLength": 10,
    "SequenceLength": 12,
    "WorkerId": 7
  }
}
```

```csharp
builder.Services
    .AddSnowflakeIds()
    .BindConfiguration("Snowflake");
```

### 4. Manual composition (console or tests)

The project also exposes the concrete types if you prefer to wire them manually:

```csharp
var settings = Options.Create(new Settings
{
    StartYear = 2025,
    StartMonth = 1,
    StartDay = 1,
    TimestampLength = 41,
    WorkerLength = 10,
    SequenceLength = 12,
    WorkerId = 1
});

var timestamp = new TimestampGenerator(settings);
var sequences = new SequenceManager(settings, timestamp);
var composer = new SnowflakeComposer(settings);
var generator = new IdGenerator(settings, sequences, timestamp, composer);

var id = generator.Next();
Console.WriteLine($"Generated ID: {id}");
```

## Settings reference

| Property | Description | Typical value |
| --- | --- | --- |
| `StartYear`, `StartMonth`, `StartDay` | UTC date that serves as the custom epoch. | `2025-01-01`
| `TimestampLength` | Bits reserved for the elapsed milliseconds since the epoch. Controls lifetime before wrap-around. | `41`
| `WorkerLength` | Bits reserved for the worker identifier. Determines how many unique workers you can run simultaneously. | `10`
| `WorkerId` | This node’s identifier. Must be between `0` and `2^WorkerLength - 1`. | `0…1023`
| `SequenceLength` | Bits reserved for the per-millisecond counter. Higher values increase throughput per node. | `12`

> ⚠️ If the sequence pool is exhausted within a single millisecond, the generator waits for the next millisecond before issuing more IDs.

## Architecture at a glance

```
IIdGenerator
 ├── ITimestampGenerator → TimestampGenerator (UTC milliseconds since epoch)
 ├── ISequenceManager     → SequenceManager    (per-timestamp counter)
 └── ISnowflakeComposer   → SnowflakeComposer  (bitwise packing)
```

Supporting helpers such as `WorkerValidator` ensure the configured worker ID stays within bounds before composing IDs.
