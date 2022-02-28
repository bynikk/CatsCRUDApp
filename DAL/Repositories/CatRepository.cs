using DAL.EF;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class CatRepository : IRepository<Cat>
    {
        private CatDbContext db;

        public CatRepository(CatDbContext context)
        {
            this.db = context;
        }
        public void Create(Cat item)
        {
            db.Cats.Add(new Cat { Id = item.Id, Name = item.Name, CreatedDate = DateTime.Now});
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            Cat cat = db.Cats.FirstOrDefault(x => x.Id == id);
            if (cat != null)
            {
                db.Cats.Remove(cat);
                db.SaveChanges();
            }
        }

        public IEnumerable<Cat> Find(Func<Cat, bool> predicate)
        {
            return db.Cats.Where(predicate).ToList();
        }

        public Cat? Get(int id)
        {
            return db.Cats.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Cat> GetAll()
        {
            return db.Cats;
        }

        public void Update(Cat item)
        {
            Cat cat = db.Cats.FirstOrDefault(p => p.Id == item.Id);

            cat.Id = item.Id;
            cat.Name = item.Name;

            db.SaveChanges();
        }
    }
}
