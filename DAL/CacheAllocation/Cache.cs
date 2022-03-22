using BLL.Entities;
using BLL.Interfaces;

namespace DAL.CacheAllocation
{
    public class Cache : ICache<Cat>
    {
        private Dictionary<int, WeakReference> cacheDictionary;

        public Cache()
        {
            cacheDictionary = new ();
        }
        public void Add(int key, Cat value)
        {
            cacheDictionary.Add(key, new WeakReference(value));
        }

        public Cat? Get(int key)
        {
            if (cacheDictionary.ContainsKey(key) && cacheDictionary[key].IsAlive)
            {
                return cacheDictionary[key].Target as Cat;
            }
            return null;
        }

        public void Delete(int key)
        {
            cacheDictionary.Remove(key);
        }
    }
}
