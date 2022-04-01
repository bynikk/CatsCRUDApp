using StackExchange.Redis;

namespace BLL.Interfaces.Cache
{
    public interface IChannelConsumer<T> where T : class
    {
        public Task<T> Read();
        public Task<bool> WaitToRead();
    }
}
