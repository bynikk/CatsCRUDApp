using BLL.Interfaces.Cache;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace DAL.CacheAllocation.Producers
{
    public class ChannelProducer : IChannelProducer<NameValueEntry[]>
    {
        IChannelContext<NameValueEntry[]> channelContext;
        Channel<NameValueEntry[]> channel;
        public ChannelProducer(IChannelContext<NameValueEntry[]> channelContext)
        {
            this.channelContext = channelContext;
            this.channel = channelContext.GetChannel();
        }

        public void Write(NameValueEntry[] item)
        {
            channel.Writer.WriteAsync(item);
        }
    }
}
