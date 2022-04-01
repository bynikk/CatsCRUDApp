using BLL.Entities;

namespace BLL.Interfaces.Cache
{
    public interface IRedisProducer
    {
        public void AddInsertCommand(Cat item);
        public void AddDeleteCommand(int key);
    }
}
