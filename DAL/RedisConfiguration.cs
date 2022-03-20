using BLL.Interfaces;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
