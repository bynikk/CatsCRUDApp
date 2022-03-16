using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ICacheService
    {
        public void Set<T>(string key, T value, TimeSpan time) where T : class;
        public T Get<T>(string key) where T : class;
    }
}
