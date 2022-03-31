using BLL.Entities;
using BLL.Interfaces.Cache;
using CSRedis;

namespace DAL.CacheAllocation.Producers
{
    public class RedisProducer : IRedisProducer
    {
        const string streamName = "telemetry";

        RedisClient client;

        public RedisProducer()
        {
            client = new RedisClient("localhost", 6379);
        }

        public void AddInsertCommand(Cat item)
        {
            lock (client)
            {
                client.Connect(4000);

                client.Publish(streamName,
                    $"{FieldNames.Command}={CommandTypes.Insert};" +
                    $"{FieldNames.Id}={item.Id};" +
                    $"{FieldNames.Name}={item.Name};" +
                    $"{FieldNames.CreationDate}={item.CreatedDate};");

                client.Quit();
            }
        }

        public void AddDeleteCommand(int key)
        {
            lock (client)
            {
                client.Connect(4000);

                client.Publish(streamName,
                    $"{FieldNames.Command}={CommandTypes.Delete};" +
                    $"{FieldNames.Id}={key};");

                client.Quit();
            }
        }
    }
}
