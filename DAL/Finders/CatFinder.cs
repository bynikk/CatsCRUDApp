using BLL.Entities;
using BLL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Finders
{
    public class CatFinder : IFinder<Cat>
    {
        private IPetsContext context;

        public CatFinder(IPetsContext context)
        {
            this.context = context;
        }

        public Task<Cat?> GetById(Cat cat)
        {
            return context.Cats.FindOne();
        }
    }
}
