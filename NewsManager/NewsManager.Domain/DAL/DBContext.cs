using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using NewsManager.Domain.Entities;

namespace NewsManager.Domain.DAL
{
    public class DBContext : DbContext
    {
        public DBContext()
            : base("NewsDB")
        {
        }

        public DbSet<News> News { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
