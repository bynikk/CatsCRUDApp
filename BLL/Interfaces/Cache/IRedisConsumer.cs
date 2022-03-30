using StackExchange.Redis;

namespace BLL.Interfaces.Cache
{
    public interface IRedisConsumer
    {
        public NameValueEntry[]? GetLastHandledElement();
    }
}
