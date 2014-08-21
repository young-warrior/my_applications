using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using NewsManager.Domain.Entities;
using NewsManager.Domain;

namespace NewsManager.Domain.DAL
{
    public class DBContext : DbContext
    {
        public DBContext()
            : base("NewsDB")
        {
//            Database.SetInitializer<DBContext>(new CreateDatabaseIfNotExists<DBContext>());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DBContext, Migrations.Configuration>("NewsDB"));
            
        }

        public  DbSet<News> News { get; set; }

        public DbSet<CategoryNews> CategoriesNews { get; set; }

        //Custom DB initalizer
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
