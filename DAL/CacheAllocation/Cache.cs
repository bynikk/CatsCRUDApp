using BLL.Entities;
using BLL.Interfaces;
using StackExchange.Redis;

namespace DAL.CacheAllocation
{
    public class Cache : ICache<Cat>
    {
        private Dictionary<int, WeakReference> cacheDictionary;

        CancellationTokenSource TokenSource;
        CancellationToken Token;
        ConnectionMultiplexer connectionMultiplexer;

        IDatabase db;

        TimeSpan expiry = TimeSpan.FromSeconds(30);

        const string streamName = "telemetry";
        const string groupName = "avg";

        public Cache()
        {

            cacheDictionary = new ();
            connectionMultiplexer = ConnectionMultiplexer.Connect("localhost:6379");
            this.db = connectionMultiplexer.GetDatabase();

            TokenSource = new CancellationTokenSource();
            Token = TokenSource.Token;

            if (!(db.KeyExists(streamName)) || (db.StreamGroupInfo(streamName)).All(x => x.Name != groupName))
            {
                db.StreamCreateConsumerGroup(streamName, groupName, "0-0", true);
            }
        }

        private Cat ParseResult(NameValueEntry[] values)
        {
            var cat = new Cat();
            switch (values.Length)
            {
                case 4:
                    cat = new Cat
                    {
                        Id = Convert.ToInt32(values[1].Value),
                        Name = values[2].Value,
                        CreatedDate = Convert.ToDateTime(values[3].Value),
                    };

                    break;
                case 2:
                    cat = new Cat
                    {
                        Id = Convert.ToInt32(values[1].Value),
                    };
                    break;
            }
            return cat;
        }

        public void Set(Cat item)
        {
            db.StreamAdd(streamName, new NameValueEntry[] {
                new NameValueEntry(FieldNames.Command, CommandTypes.Insert),
                new NameValueEntry(FieldNames.Id, item.Id),
                new NameValueEntry(FieldNames.Name, item.Name),
                new NameValueEntry(FieldNames.CreationDate, item.CreatedDate.ToString()),
            });
        }

        public Cat? Get(int key)
        { 
            if (!cacheDictionary.ContainsKey(key))
            {
                return null;
            }

            if (!cacheDictionary[key].IsAlive)
            {
                Delete(key);
                return null;
            }

            return cacheDictionary[key].Target as Cat;            
        }

        public void Delete(int key)
        {
            var result = db.StreamRange(streamName, "-", "+").FirstOrDefault(c => c.Values[0].Value == key);

            db.StreamAdd(streamName, new NameValueEntry[] {
                new NameValueEntry(FieldNames.Command, CommandTypes.Delete),
                new NameValueEntry(FieldNames.Id, key),

            });

            if (!result.IsNull) db.StreamDelete(streamName, new RedisValue[] { result.Id });
        }

        public async void ListenTask()
        {
            var handledResult = await db.StreamRangeAsync(streamName, "-", "+", 1, Order.Descending);
            var lowestHandledId = handledResult.Last().Id;

            RedisValue token = Environment.MachineName;

            while (!Token.IsCancellationRequested)
            {
                var result = await db.StreamRangeAsync(streamName, lowestHandledId, "+", 2);
                var handleResult = result.Last();

                if (result.Any() && lowestHandledId != handleResult.Id)
                {
                    lock (db)
                    {
                        lowestHandledId = handleResult.Id;

                        var streamCat = handleResult.Values;
                        Cat cat = ParseResult(streamCat);

                        switch (streamCat[0].Value.ToString())
                        {
                            case CommandTypes.Insert:
                                Console.WriteLine($"{CommandTypes.Insert} cat at id:{cat.Id}");
                                cacheDictionary.Add(cat.Id, new WeakReference(cat));
                                break;
                            case CommandTypes.Delete:
                                Console.WriteLine($"{CommandTypes.Delete} cat at id:{cat.Id}");
                                cacheDictionary.Remove(cat.Id);
                                break;
                        }
                    }
                }
            }
        }
    }
}
