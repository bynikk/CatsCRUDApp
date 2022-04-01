using BLL.Interfaces.Cache;
using System.Threading.Channels;

namespace DAL.CacheAllocation.Producers
{
    public class ChannelProducer : IChannelProducer<CatStreamModel>
    {
        IChannelContext<CatStreamModel> channelContext;
        Channel<CatStreamModel> channel;
        public ChannelProducer(IChannelContext<CatStreamModel> channelContext)
        {
            this.channelContext = channelContext;
            this.channel = channelContext.GetChannel();
        }

        public Task Write(CatStreamModel item)
        {
            return channel.Writer.WriteAsync(item).AsTask();
        }
    }
}
