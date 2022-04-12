namespace DAL.Config;

public class MongoConfig
{
    public string Ip { get; set; }
    public int Port { get; set; }
    public string DatabaseName { get; set; }
    public string CatsTableName { get; set; }
    public string DogsTableName { get; set; }
    public string ConnectionString
    {
        get
        {
            return $@"mongodb://{Ip}:{Port}";
        }
    }

    public MongoConfig()
    {

    }

}
