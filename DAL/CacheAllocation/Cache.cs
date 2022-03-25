using BLL.Entities;
using BLL.Interfaces;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;

namespace DAL.CacheAllocation
{
    public class Cache : ICache<Cat>
    {
        private Dictionary<int, WeakReference> cacheDictionary;

        CancellationTokenSource TokenSource;
        CancellationToken Token;
        ConnectionMultiplexer connectionMultiplexer;
        List<RedLockMultiplexer> multiplexers;

        RedLockFactory redlockFactory;
        IDatabase db;

        TimeSpan expiry = TimeSpan.FromSeconds(30);
        TimeSpan wait = TimeSpan.FromSeconds(10);
        TimeSpan retry = TimeSpan.FromSeconds(1);

        const string streamName = "telemetry";
        const string groupName = "avg";

        public Cache()
        {
            this.db = connectionMultiplexer.GetDatabase();

            cacheDictionary = new ();
            connectionMultiplexer = ConnectionMultiplexer.Connect("localhost:6379");

            TokenSource = new CancellationTokenSource();
            Token = TokenSource.Token;

            multiplexers = new List<RedLockMultiplexer>
            {
                connectionMultiplexer,
            };

            if (!(db.KeyExists(streamName)) || (db.StreamGroupInfo(streamName)).All(x => x.Name != groupName))
            {
                db.StreamCreateConsumerGroup(streamName, groupName, "0-0", true);
            }
        }

        Cat ParseResult(NameValueEntry[] values)
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
                new NameValueEntry("command", "insert"),
                new NameValueEntry("id", item.Id),
                new NameValueEntry("name", item.Name),
                new NameValueEntry("date", item.CreatedDate.ToString()),
            });
        }

        public Cat? Get(int key)
        { 
            if (cacheDictionary.ContainsKey(key) && cacheDictionary[key].IsAlive)
            {
                return cacheDictionary[key].Target as Cat;
            }
            return null;
        }

        public void Delete(int key)
        {
            var result = db.StreamRange(streamName, "-", "+").FirstOrDefault(c => c.Values[0].Value == key);

            db.StreamAdd(streamName, new NameValueEntry[] {
                new NameValueEntry("command", "delete"),
                new NameValueEntry("id", key),

            });

            if (!result.IsNull) db.StreamDelete(streamName, new RedisValue[] { result.Id });
        }

        public async void ListenTask()
        {
            redlockFactory = RedLockFactory.Create(multiplexers);

            var handledResult = await db.StreamRangeAsync(streamName, "-", "+", 1, Order.Descending);
            var lowestHandledId = handledResult.Last().Id;

            var readTask = Task.Run(async () =>
            {
                while (!Token.IsCancellationRequested)
                {
                    await using (var redLock = await redlockFactory.CreateLockAsync(streamName, expiry, wait, retry))
                    {
                        if (redLock.IsAcquired)
                        {
                            var result = await db.StreamRangeAsync(streamName, lowestHandledId, "+", 2);
                            if (result.Any() && lowestHandledId != result.Last().Id)
                            {
                                lowestHandledId = result.Last().Id;

                                var streamCat = result.Last().Values;
                                Cat cat = ParseResult(streamCat);

                                switch (streamCat[0].Value.ToString())
                                {
                                    case "insert":
                                        Console.WriteLine($"Insert cat at id:{cat.Id} [{cat.Name} - {cat.CreatedDate}]");
                                        cacheDictionary.Add(cat.Id, new WeakReference(cat));
                                        break;
                                    case "delete":
                                        Console.WriteLine($"Deleted cat at id:{cat.Id}");
                                        cacheDictionary.Remove(cat.Id);
                                        break;
                                }
                            }

                            await Task.Delay(100);
                        }
                    }
                }
            });
        }
    }
}
