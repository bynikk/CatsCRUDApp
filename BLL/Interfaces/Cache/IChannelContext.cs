using System.Threading.Channels;

namespace BLL.Interfaces.Cache
{
    public interface IChannelContext<T> where T : class
    {
        public Channel<T> GetChannel();
    }
}
