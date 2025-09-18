using FluentAssertions;
using SnowflakeIds.Helpers;

namespace SnowflakeIds.Tests.Helpers;

public class WorkerValidatorTests
{
    [Fact]
    public void GivenLessThan0WorkerId_WhenValidatingWorker_ThrowsArgumentOutOfRangeException()
    {
        //Arrange
        const int workerId = -1;
        const int workerLength = 0;
        
        //Act
        Action act = () => WorkerValidator.Validate(workerId, workerLength);
        
        //Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void GivenGreaterThanMaxWorkerLength_WhenValidatingWorker_ThrowsArgumentOutOfRangeException()
    {
        //Arrange
        const int workerId = 1;
        const int workerLength = 1;
        
        //Act
        Action act = () => WorkerValidator.Validate(workerId, workerLength);
        
        //Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void GivenAValidWorkerId_WhenValidatingWorker_ReturnsWorkerIdWithinMask()
    {
        //Arrange
        const int workerId = 5;
        const int workerLength = 12;
        const long mask = (1L << workerLength) - 1;

        //Act
        var result = WorkerValidator.Validate(workerId, workerLength);

        //Assert
        result.Should().Be(workerId);
        result.Should().BeInRange(0, mask);
    }
}