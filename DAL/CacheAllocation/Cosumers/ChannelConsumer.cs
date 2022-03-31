using BLL.Interfaces.Cache;
using System.Threading.Channels;

namespace DAL.CacheAllocation.Producers
{
    public class ChannelConsumer : IChannelConsumer<CatStreamModel>
    {
        IChannelContext<CatStreamModel> channelContext;
        Channel<CatStreamModel> channel;

        public ChannelConsumer(IChannelContext<CatStreamModel> channelContext)
        {
            this.channelContext = channelContext;
            this.channel = channelContext.GetChannel();
        }

        public Task<CatStreamModel> Read()
        {
            return channel.Reader.ReadAsync().AsTask();
        }

        public Task<bool> WaitToRead()
        {
            return channel.Reader.WaitToReadAsync().AsTask();
        }
    }
}
