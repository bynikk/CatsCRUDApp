﻿using DAL.EF;
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
        public async Task Create(Cat item)
        {
            await db.Cats.AddAsync(new Cat { Id = item.Id, Name = item.Name, CreatedDate = DateTime.Now });
            await Task.Run(() => db.SaveChangesAsync());
        }

        public async Task Delete(int id)
        {
            var cat = await db.Cats.FirstOrDefaultAsync(x => x.Id == id);
            if (cat != null)
            {
                db.Cats.Remove(cat);
                await Task.Run(() => db.SaveChangesAsync());
            }
        }

        public async Task<IEnumerable<Cat>> Find(Func<Cat, bool> predicate)
        {
            return db.Cats.Where(predicate);
        }

        public async Task<Cat?> Get(int id)
        {
            return await Task.Run(() => db.Cats.FirstOrDefaultAsync(x => x.Id == id));
        }

        public async Task<IEnumerable<Cat>> GetAll()
        {
            return db.Cats;
        }

        public async Task Update(Cat item)
        {
            var cat = await db.Cats.FirstOrDefaultAsync(p => p.Id == item.Id);

            cat.Id = item.Id;
            cat.Name = item.Name;

            await Task.Run(() => db.SaveChangesAsync());
        }
    }
}