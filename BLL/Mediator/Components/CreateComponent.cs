using BLL.Entities;
using BLL.Mediator.Flags;

namespace BLL.Mediator.Components
{
    public class CreateComponent : BaseComponent
    {
        public void Create(Cat cat)
        {
            Console.WriteLine("Component does create.");

            this._mediator.Notify(this, MediatorFlag.Create, cat);
        }
    }
}
