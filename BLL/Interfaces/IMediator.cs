using BLL.Entities;
using BLL.Mediator.Flags;

namespace BLL.Interfaces
{
    public interface IMediator
    {
        void Notify(object sender, MediatorFlag ev, Cat cat);
        void Notify(object sender, MediatorFlag ev, int id);
        public Task<List<Cat>> Notify(object sender, MediatorFlag ev, string flag);
    }
}
