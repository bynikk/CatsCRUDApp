namespace DAL.Config;

public class RedisConfig
{
    public RedisConfig()
    {
        Ip = "127.0.0.1";
        Port = 6379;
        StreamName = "telemetry";
        ListenExpiryTime = 4000;
    }

    public string Ip { get; set; }
    public int Port { get; set; }
    public int ListenExpiryTime { get; set; }
    public string StreamName { get; set; }

}
