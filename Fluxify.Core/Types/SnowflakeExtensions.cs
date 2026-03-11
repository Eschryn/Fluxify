namespace Fluxify.Core;

public static class SnowflakeExtensions
{
    extension(Snowflake snowflake)
    {
        private ulong EpochMs => (ulong)snowflake >> 22;
        public long UnixEpochMs => (long)(snowflake.EpochMs + 1420070400000);
        public byte WorkerId => (byte)(((ulong)snowflake & 0x3E0000) >> 17);
        public byte InternalProcessId => (byte)(((ulong)snowflake & 0x1F000) >> 12);
        public ushort ScopedId => (ushort)((ulong)snowflake & 0xFFF);
    }
}