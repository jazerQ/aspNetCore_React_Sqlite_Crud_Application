using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class MVDbContext : DbContext
    {
        public MVDbContext(DbContextOptions<MVDbContext> options) : base(options)
        {}
        public DbSet<BookEntity> Books { get; set; }
    }
}
