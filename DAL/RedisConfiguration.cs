using BLL.Interfaces;
namespace DAL
{
    public class RedisConfiguration : IRedisConfiguration
    {
        public string Host
        {
            get
            {
                return "localhost";
            }
        }

        public int Port
        {
            get
            {
                return 6379;
            }
        }

        public TimeSpan expirationTime
        {
            get
            {
                return TimeSpan.FromSeconds(300);
            }
        }
    }
}
