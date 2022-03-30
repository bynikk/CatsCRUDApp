using BLL.Interfaces.Cache;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.CacheAllocation.Cosumers
{
    public class RedisConsumer : IRedisConsumer
    {
        ConnectionMultiplexer connectionMultiplexer;
        IDatabase db;

        const string streamName = "telemetry";

        RedisValue lastHandledId;

        public RedisConsumer()
        {
            this.connectionMultiplexer = ConnectionMultiplexer.Connect("localhost:6379");
            this.db = connectionMultiplexer.GetDatabase();
            lastHandledId = GetLastId();
        }

        private RedisValue GetLastId()
        {
            var handledResult = db.StreamRange(streamName, "-", "+", 1, Order.Descending);
            return handledResult.Last().Id;
        }

        public NameValueEntry[]? GetLastHandledElement()
        {
            var result = db.StreamRange(streamName, lastHandledId, "+", 2);
            if (result.Any() && lastHandledId != result.Last().Id)
            {
                lastHandledId = result.Last().Id;
                return result.Last().Values;
            }
            return null;

            //var a = db.Execute("XLEN", streamName);

            //if (a.IsNull)
            //{
            //    return null;
            //}

            //RedisResult streams;

            //streams = db.Execute("XREAD", "BLOCK", "4000", "STREAMS", "telemetry", "$");

            //if (streams.IsNull)
            //{
            //    return null;
            //}

            //// need to parse
            //var firstLayer = (RedisResult[])streams;

            //foreach (var key in streams.ToDictionary().Keys)
            //{
            //    _ = streams.ToDictionary()[key];
            //}

            //return new NameValueEntry[0];
        }
    }
}
