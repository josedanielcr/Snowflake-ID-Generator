using System.Collections.Concurrent;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using SnowflakeIds.Common;
using SnowflakeIds.Implementations;
using SnowflakeIds.Tests.Shared.Fixtures;

namespace SnowflakeIds.Tests.Implementations;

public class SequenceManagerTests(SnowflakeSutFixture fx) : IClassFixture<SnowflakeSutFixture>
{
    [Fact]
    public void GivenValidTimestamp_WhenGeneratingSequence_ThenSequenceIsReturned()
    {
        //Arrange
        var (_, timestampManager, sequenceManager, _) = fx.CreateAll();
        var timestamp = timestampManager.Generate();
        //Act
        var sequence = sequenceManager.GetSequence(ref timestamp);

        //Assert
        sequence.Should().NotBe(null);
    }

    [Fact]
    public void GivenSameTimestamps_WhenGeneratingSequence_ThenTwoDifferentSequencesAreReturned()
    {
        //Arrange
        var (_, timestampManager, sequenceManager, _) = fx.CreateAll();
        var timestamp = timestampManager.Generate();
        
        //Act
        var sequence1 = sequenceManager.GetSequence(ref timestamp);
        var sequence2 = sequenceManager.GetSequence(ref timestamp);

        //Assert
        sequence1.Should().NotBe(sequence2);
        sequence1.Should().Be(0);
        sequence2.Should().Be(1);
    }

    [Fact]
    public async Task GivenTwoThreads_WhenGeneratingSequence_ThenTwoDifferentSequencesAreReturned()
    {
        // Arrange
        var (_, timestampManager, sequenceManager, _) = fx.CreateAll();
        
        var baseTimestamp = timestampManager.Generate();
        var call1 = new SequenceCall(sequenceManager, baseTimestamp);
        var call2 = new SequenceCall(sequenceManager, baseTimestamp);
        var startGate = new ManualResetEventSlim(false);
        var results = new ConcurrentBag<int>();
        var t1 = Task.Run(() =>
        {
            startGate.Wait();
            call1.Invoke();
            results.Add(call1.Result);
        });

        var t2 = Task.Run(() =>
        {
            startGate.Wait();
            call2.Invoke();
            results.Add(call2.Result);
        });
        
        //Act
        startGate.Set();
        await Task.WhenAll(t1, t2);
        
        //Assert
        results.Count.Should().Be(2);
        results.Should().OnlyHaveUniqueItems();
        results.Should().BeSubsetOf(new[] { 0, 1 });
    }
}

internal class SequenceCall(SequenceManager sm, long ts)
{
    private long _timestamp = ts;
    public int Result;
    public void Invoke()
    {
        Result = sm.GetSequence(ref _timestamp);
    }
}