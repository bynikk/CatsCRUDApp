using BLL.Entities;
using DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace CatsCRUDAppTest
{

    public class FakeDbContext : CatDbContext
    {
        public FakeDbContext(DbContextOptions<CatDbContext> options) : base(options)
        {
        }
    }
    
}
