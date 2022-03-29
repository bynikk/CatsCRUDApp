using BLL.Entities;
using BLL.Interfaces.Cache;
using StackExchange.Redis;

namespace DAL.CacheAllocation.Producers
{
    public class RedisProducer : IRedisProducer
    {
        ConnectionMultiplexer connectionMultiplexer;
        IDatabase db;
        const string streamName = "telemetry";

        public RedisProducer()
        {
            this.connectionMultiplexer = ConnectionMultiplexer.Connect("localhost:6379");
            this.db = connectionMultiplexer.GetDatabase();
        }

        public void AddInsertCommand(Cat item)
        {
            db.StreamAdd(streamName, new NameValueEntry[] {
                new NameValueEntry(FieldNames.Command, CommandTypes.Insert),
                new NameValueEntry(FieldNames.Id, item.Id),
                new NameValueEntry(FieldNames.Name, item.Name),
                new NameValueEntry(FieldNames.CreationDate, item.CreatedDate.ToString()),
            });
        }

        public void AddDeleteCommand(int key)
        {
            var result = db.StreamRange(streamName, "-", "+").FirstOrDefault(c => c.Values[0].Value == key);

            db.StreamAdd(streamName, new NameValueEntry[] {
                new NameValueEntry(FieldNames.Command, CommandTypes.Delete),
                new NameValueEntry(FieldNames.Id, key),
            });

            if (!result.IsNull) db.StreamDelete(streamName, new RedisValue[] { result.Id });
        }
    }
}
