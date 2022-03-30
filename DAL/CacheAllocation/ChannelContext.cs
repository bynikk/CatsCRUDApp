using BLL.Interfaces.Cache;
using System.Threading.Channels;

namespace DAL.CacheAllocation
{
    public class ChannelContext : IChannelContext<CatStreamModel>
    {
        Channel<CatStreamModel> channel;

        public ChannelContext()
        {
            channel = Channel.CreateUnbounded<CatStreamModel>();
        }

        public Channel<CatStreamModel> GetChannel()
        {
            return channel;
        }
    }
}
