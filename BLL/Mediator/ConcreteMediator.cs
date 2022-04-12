using BLL.Entities;
using BLL.Interfaces;
using BLL.Mediator.Components;
using BLL.Mediator.Flags;

namespace BLL.Mediator
{
    public class ConcreteMediator : IMediator
    {
        private CreateComponent createComponent;
        private UpdateComponent updateComponent;
        private DeleteComponent deleteComponent;
        private GetComponent getComponent;

        private ICatService catService;

        public ConcreteMediator(
            CreateComponent createComponent,
            UpdateComponent updateComponent,
            DeleteComponent deleteComponent,
            GetComponent getComponent,
            ICatService catService)
        {
            this.createComponent = createComponent;
            this.updateComponent = updateComponent;
            this.deleteComponent = deleteComponent;
            this.getComponent = getComponent;

            this.createComponent.SetMediator(this);
            this.updateComponent.SetMediator(this);
            this.deleteComponent.SetMediator(this);
            this.getComponent.SetMediator(this);

            this.catService = catService;
        }

        public void Notify(object sender, MediatorFlag ev, Cat? cat)
        {
            switch (ev)
            {
                case MediatorFlag.Create:
                    catService.Create(cat);
                    break;
                case MediatorFlag.Update:
                    catService.Update(cat);
                    break;
                default:
                    break;
            }
        }

        public void Notify(object sender, MediatorFlag ev, int catId)
        {
            switch (ev)
            {
                case MediatorFlag.Delete:
                    catService.Delete(catId);
                    break;
                default:
                    break;
            }
        }

        public Task<List<Cat>> Notify(object sender, MediatorFlag ev, string flag)
        {
            if (flag == "all" && ev == MediatorFlag.GetAll) return catService.Get();
            return null;
        }

        private Cat? Switch(object obj) =>
            obj switch
            {
                Cat => obj as Cat,
                _ => null
            };
    }
}
