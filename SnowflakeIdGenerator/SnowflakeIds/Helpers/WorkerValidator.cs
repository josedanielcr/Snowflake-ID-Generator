namespace SnowflakeIds.Helpers;

public static class WorkerValidator
{
    public static long Validate(int workerId, int workerLength)
    {
        var mask = (1L << workerLength) - 1;
        ArgumentOutOfRangeException.ThrowIfLessThan(workerId,0,$"WorkerId {workerId} less than 0");
        ArgumentOutOfRangeException.ThrowIfGreaterThan(workerId,mask, $"WorkerId {workerId} greater than {mask}");
        return workerId & mask;
    }
}