using BLL.Entities;
using BLL.Interfaces;
using StackExchange.Redis;
using System.Text;

namespace DAL.CacheAllocation
{
    public class Cache : ICache<Cat>
    {
        private Dictionary<int, WeakReference> cacheDictionary;

        CancellationTokenSource TokenSource;
        CancellationToken Token;
        ConnectionMultiplexer connectionMultiplexer;

        IDatabase db;

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

        Dictionary<string, string> ParseResult(StreamEntry entry) => entry.Values.ToDictionary(x => x.Name.ToString(), x => x.Value.ToString());

        public void Set(Cat item)
        {
            cacheDictionary.Add(item.Id, new WeakReference(item));

            db.StreamAdd(streamName, new NameValueEntry[] {
                new NameValueEntry("command", "insert"),
                new NameValueEntry("id", item.Id),
            });

            db.StreamAdd(streamName, new NameValueEntry[] {
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


            cacheDictionary.Remove(key);
        }

        public async void ListenTask()
        {
            var handledResult = await db.StreamRangeAsync(streamName, "-", "+", 1, Order.Descending);
            var lowestHandledId = handledResult.Last().Id;

            var readTask = Task.Run(async () =>
            {
                while (!Token.IsCancellationRequested)
                {
                    var result = await db.StreamRangeAsync(streamName, lowestHandledId, "+", 3);

                    //var res = await db.StreamReadAsync(streamName, "$", 1);

                    //if (result.Any())
                    if (result.Any() && lowestHandledId != result.Last().Id)
                    {
                        var dict = ParseResult(result.Last());
                        var sb = new StringBuilder();
                        foreach (var key in dict.Keys)
                        {
                            sb.Append(dict[key]);
                        }

                        Console.WriteLine(sb.ToString());
                        lowestHandledId = result.Last().Id;

                    }

                    await Task.Delay(1000);
                }
            });
        }
    }
}
