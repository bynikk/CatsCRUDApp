using BLL.Entities;
using BLL.Interfaces;
using BLL.Interfaces.Cache;
using DAL.CacheAllocation.Cosumers;
using StackExchange.Redis;
using System.Threading.Channels;

namespace DAL.CacheAllocation
{
    public class Cache : ICache<Cat>
    {
        private Dictionary<int, WeakReference> cacheDictionary;

        CancellationTokenSource tokenSource;
        CancellationToken Token;

        Channel<NameValueEntry[]> channel;

        IRedisProducer redisProducer;
        RedisConsumer redisComsumer;

        const string streamName = "telemetry";

        public Cache(IRedisProducer redisProducer)
        {
            cacheDictionary = new();
            this.redisProducer = redisProducer;
            redisComsumer = new RedisConsumer();

            tokenSource = new CancellationTokenSource();
            Token = tokenSource.Token;

            channel = Channel.CreateUnbounded<NameValueEntry[]>();
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
            redisProducer.AddInsertCommand(item);
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
            redisProducer.AddDeleteCommand(key);
        }

        public async void ListenRedisTask()
        {
            while (!Token.IsCancellationRequested)
            {
                var lastHandledElement = redisComsumer.GetLastHandledElement();

                if (lastHandledElement != null)
                {
                    lock (channel)
                    {
                        channel.Writer.WriteAsync(lastHandledElement);
                    }
                }

                await Task.Delay(1);
            }
        }

        public async void ListenChannelTask()
        {
            while (!Token.IsCancellationRequested)
            {
                if (await channel.Reader.WaitToReadAsync())
                {
                    var streamCat = await channel.Reader.ReadAsync();

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
