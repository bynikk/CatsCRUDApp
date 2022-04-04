using BLL.Interfaces.Cache;
using CSRedis;

namespace DAL.CacheAllocation.Cosumers;

public class RedisConsumer : IRedisConsumer
{
    const string streamName = "telemetry";
    int expiryTime = 4000;
    RedisClient client;

    public RedisConsumer()
    {
        client = new RedisClient("redis", 6379);

        client.SubscriptionReceived += (sender, data) => 
        {
            OnDataReceived?.Invoke(this, data.Message.Body);
        };
    }

    public EventHandler<string> OnDataReceived { get; set; }

    public void WaitToGetNewElement()
    {
        if (!client.IsConnected)
        {
            client.Connect(expiryTime);
        }

        client.Subscribe(streamName);
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
