namespace BLL.Interfaces
{
    public interface IRedisConfiguration
    {
        public string Host { get; }
        public int Port { get; }
        public TimeSpan expirationTime { get; }
    }
}
