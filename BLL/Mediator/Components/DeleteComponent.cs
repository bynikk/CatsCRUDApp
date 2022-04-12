using BLL.Entities;
using BLL.Mediator.Flags;

namespace BLL.Mediator.Components
{
    public class DeleteComponent : BaseComponent
    {
        public void Delete(int id)
        {
            Console.WriteLine("Component does delete.");

            this._mediator.Notify(this, MediatorFlag.Delete, id);
        }
    }
}
