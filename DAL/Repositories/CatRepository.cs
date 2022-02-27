using DAL.EF;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class CatRepository : IRepository<Cat>
    {
        private ShelterDbContext db;

        public CatRepository(ShelterDbContext context)
        {
            this.db = context;
        }
        public void Create(Cat item)
        {
            db.Cats.Add(item);
        }

        public void Delete(int id)
        {
            Cat cat = db.Cats.Find(id);
            if (cat != null)
                db.Cats.Remove(cat);
        }

        public IEnumerable<Cat> Find(Func<Cat, bool> predicate)
        {
            return db.Cats.Where(predicate).ToList();
        }

        public Cat Get(int id)
        {
            return db.Cats.Find(id);
        }

        public IEnumerable<Cat> GetAll()
        {
            return db.Cats;
        }

        public void Update(Cat item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
