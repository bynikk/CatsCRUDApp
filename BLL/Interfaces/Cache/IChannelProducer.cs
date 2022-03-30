using StackExchange.Redis;

namespace BLL.Interfaces.Cache
{
    public interface IChannelProducer<T> where T : class
    {
        public void Write(T item);
    }
}
