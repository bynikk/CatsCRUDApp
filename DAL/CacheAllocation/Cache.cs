using BLL.Entities;
using BLL.Interfaces;
using BLL.Interfaces.Cache;
using StackExchange.Redis;

namespace DAL.CacheAllocation
{
    public class Cache : ICache<Cat>
    {
        private Dictionary<int, WeakReference> cacheDictionary;

        CancellationTokenSource tokenSource;
        CancellationToken Token;

        IRedisProducer redisProducer;
        IRedisConsumer redisComsumer;

        IChannelProducer<CatStreamModel> channelProducer;
        IChannelConsumer<CatStreamModel> channelComsumer;

        const string streamName = "telemetry";

        public Cache(
            IRedisProducer redisProducer,
            IRedisConsumer redisConsumer,
            IChannelProducer<CatStreamModel> channelProducer,
            IChannelConsumer<CatStreamModel> channelComsumer)
        {
            cacheDictionary = new();
            this.redisProducer = redisProducer;
            this.redisComsumer = redisConsumer;

            this.channelProducer = channelProducer;
            this.channelComsumer = channelComsumer;

            tokenSource = new CancellationTokenSource();
            Token = tokenSource.Token;
        }

        private CatStreamModel ParseResult(Dictionary<string, string> dict)
        {
            var cat = new CatStreamModel();
            switch (dict.Count)
            {
                case 4:
                    cat = new CatStreamModel
                    {
                        Command = dict[FieldNames.Command],
                        Id = Convert.ToInt32(dict[FieldNames.Id]),
                        Name = dict[FieldNames.Name],
                        CreatedDate = Convert.ToDateTime(dict[FieldNames.CreationDate]),
                    };

                    break;
                case 2:
                    cat = new CatStreamModel
                    {
                        Command = dict[FieldNames.Command],
                        Id = Convert.ToInt32(dict[FieldNames.Id]),
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
            if (cacheDictionary.ContainsKey(key) && cacheDictionary[key].IsAlive)
            {
                return cacheDictionary[key].Target as Cat;
            }
            return null;
        }

        public void Delete(int key)
        {
            redisProducer.AddDeleteCommand(key);
        }

        public async void ListenRedisTask()
        {
            redisComsumer.OnDataReceived += (sender, message) =>
            {
                Console.WriteLine(message);
                var dict = message.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
               .Select(part => part.Split('='))
               .ToDictionary(split => split[0], split => split[1]);

                channelProducer.Write(ParseResult(dict));
            };

            await Task.Run(() => redisComsumer.WaitToGetNewElement());
        }

        

        public async void ListenChannelTask()
        {
            while (!Token.IsCancellationRequested)
            {
                if (await channelComsumer.WaitToRead())
                {
                    var streamCat = await channelComsumer.Read();
                    lock (cacheDictionary)
                    {

                        Cat cat = new Cat { Id = streamCat.Id, Name = streamCat.Name, CreatedDate = streamCat.CreatedDate };

                        switch (streamCat.Command)
                        {
                            case CommandTypes.Insert:
                                Console.WriteLine($"{CommandTypes.Insert} cat at id:{cat.Id}");
                                cacheDictionary.Add(streamCat.Id, new WeakReference(cat));
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
