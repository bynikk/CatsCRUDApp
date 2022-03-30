using BLL.Interfaces.Cache;
using StackExchange.Redis;
using System.Threading.Channels;

namespace DAL.CacheAllocation.Producers
{
    public class ChannelConsumer : IChannelConsumer<NameValueEntry[]>
    {
        IChannelContext<NameValueEntry[]> channelContext;
        Channel<NameValueEntry[]> channel;

        public ChannelConsumer(IChannelContext<NameValueEntry[]> channelContext)
        {
            this.channelContext = channelContext;
            this.channel = channelContext.GetChannel();
        }

        public Task<NameValueEntry[]> Read()
        {
            return channel.Reader.ReadAsync().AsTask();
        }

        public Task<bool> WaitToRead()
        {
            return channel.Reader.WaitToReadAsync().AsTask();
        }
    }
}
