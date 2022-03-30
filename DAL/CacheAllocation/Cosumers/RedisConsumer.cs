using BLL.Interfaces.Cache;
using CSRedis;

namespace DAL.CacheAllocation.Cosumers
{
    public class RedisConsumer : IRedisConsumer
    {
        const string streamName = "telemetry";

        CSRedisClient client;

        public RedisConsumer()
        {
            client = new CSRedisClient("localhost:6379");
        }

        public Dictionary<string, string>? GetLastHandledElement()
        {
            var result = client.XRead(1, 400, new (string key, string id)[] { new(streamName, "$") });
            if (result != null)
            {
                Task.Delay(1000);
                return parse(result[0]);
            }

            return null;
        }

        Func<(string key, (string id, string[] items)[] data), Dictionary<string, string>> parse = delegate ((string key, (string id, string[] items)[] data) streamResult)
        {
            var message = streamResult.data.First().items;
            var result = new Dictionary<string, string>();
            for (var i = 0; i < message.Length; i += 2)
            {
                result.Add(message[i], message[i + 1]);
            }

            return result;
        };
    }
}
