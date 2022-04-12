namespace DAL.Config;

public class RedisConfig
{
    public RedisConfig()
    {

    }

    public string Ip { get; set; }
    public int Port { get; set; }
    public int ListenExpiryTime { get; set; }
    public string StreamName { get; set; }

}
