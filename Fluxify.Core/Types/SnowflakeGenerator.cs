namespace Fluxify.Core.Types;

public class SnowflakeGenerator(byte processId, byte workerId)
{
    public static SnowflakeGenerator Default { get; } = new(0, 0);
    
    private uint _processCounter = 0;
    public Snowflake Create()
    {
        var timestamp = (ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - 1420070400000;
        var increment = (ulong)Interlocked.Increment(ref _processCounter);
        
        return new Snowflake((timestamp << 22) | ((ulong)workerId << 17 & 0x1F) | ((ulong)(processId & 0x1F) << 12) | increment & 0xFFF);
    }
}