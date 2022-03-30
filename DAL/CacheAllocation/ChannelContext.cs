using BLL.Interfaces.Cache;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace DAL.CacheAllocation
{
    public class ChannelContext : IChannelContext<NameValueEntry[]>
    {
        Channel<NameValueEntry[]> channel;

        public ChannelContext()
        {
            channel = Channel.CreateUnbounded<NameValueEntry[]>();
        }

        public Channel<NameValueEntry[]> GetChannel()
        {
            return channel;
        }
    }
}
