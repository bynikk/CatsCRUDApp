using BLL.Entities;
using BLL.Mediator.Flags;

namespace BLL.Mediator.Components
{
    public class UpdateComponent : BaseComponent
    {
        public void Update(Cat cat)
        {
            Console.WriteLine("Component does Update.");

            this._mediator.Notify(this, MediatorFlag.Update, cat);
        }
    }
}
