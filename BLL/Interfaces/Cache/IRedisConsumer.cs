namespace BLL.Interfaces.Cache
{
    public interface IRedisConsumer
    {
        public EventHandler<string> OnDataReceived { get; set; }

        public void WaitToGetNewElement();
    }
}
