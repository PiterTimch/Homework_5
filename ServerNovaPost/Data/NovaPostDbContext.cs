using Microsoft.EntityFrameworkCore;
using ServerNovaPost.Constants;
using ServerNovaPost.Data.Entities;

namespace ServerNovaPost.Data
{
    public class NovaPostDbContext : DbContext
    {
        public DbSet<AreaEntity> Areas { get; set; }
        public DbSet<CityEntity> Cities { get; set; }
        public DbSet<DepartmentEntity> Departments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(AppDatabase.ConnectionString);
        }
    }
}
