using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Twitter.Models;

namespace Twitter.DAL
{
    public class TwitterContext: DbContext
    {
        public TwitterContext(): base("TwitterContext")
        {
            Database.SetInitializer<TwitterContext>(new CreateDatabaseIfNotExists<TwitterContext>());
        }

        public DbSet<Person> Person { get; set; }
        public DbSet<Following> Following { get; set; }
		public DbSet<Models.Twitter> Twitter { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();            
        }
    }
}