using BLL.Entities;
using BLL.Mediator.Flags;

namespace BLL.Mediator.Components
{
    public class GetComponent : BaseComponent
    {
        public Task<List<Cat>> Get()
        {
            Console.WriteLine("Component does get.");

            return this._mediator.Notify(this, MediatorFlag.GetAll, "all");
        }
    }
}
