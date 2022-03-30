namespace BLL.Interfaces.Cache
{
    public interface IRedisConsumer
    {
        public Dictionary<string, string>? GetLastHandledElement();
    }
}
