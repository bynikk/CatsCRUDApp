namespace DAL.Config;

public class Ipconfig
{
    public Ipconfig()
    {
        RedisIp = "127.0.0.1";
        RedisPort = 6379;
        MongoIp = "127.0.0.1";
        MongoPort = 27017;
    }

    public string RedisIp { get; set; }

    public int RedisPort { get; set; }

    public string MongoIp { get; set; }

    public int MongoPort { get; set; }
}
