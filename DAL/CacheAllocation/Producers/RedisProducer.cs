using BLL.Entities;
using BLL.Interfaces.Cache;
using StackExchange.Redis;
using CSRedis;

namespace DAL.CacheAllocation.Producers
{
    public class RedisProducer : IRedisProducer
    {
        ConnectionMultiplexer connectionMultiplexer;
        IDatabase db;
        const string streamName = "telemetry";

        CSRedisClient client;

        public RedisProducer()
        {
            client = new CSRedisClient("localhost:6379");
            //this.connectionMultiplexer = ConnectionMultiplexer.Connect("localhost:6379");
            //this.db = connectionMultiplexer.GetDatabase();
        }

        public void AddInsertCommand(Cat item)
        {
            client.XAdd(streamName, new (string, string)[] 
            { new (FieldNames.Command, CommandTypes.Insert),
              new (FieldNames.Id, item.Id.ToString()),
              new (FieldNames.Name, item.Name),
              new (FieldNames.CreationDate, item.CreatedDate.ToString()),
            });
        }

        public void AddDeleteCommand(int key)
        {
            client.XAdd(streamName, new (string, string)[]
            { new (FieldNames.Command, CommandTypes.Delete),
              new (FieldNames.Id, key.ToString()),
            });
        }
    }
}
