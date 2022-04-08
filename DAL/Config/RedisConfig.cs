namespace DAL.Config;

public class RedisConfig
{
    public RedisConfig()
    {
        Ip = "127.0.0.1";
        Port = 6379;
    }

    public string Ip { get; set; }

    public int Port { get; set; }
}
