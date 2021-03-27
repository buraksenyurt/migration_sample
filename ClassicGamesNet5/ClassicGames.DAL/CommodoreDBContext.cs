using ClassicGames.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ClassicGames.DAL
{
    public class CommodoreDBContext
        :DbContext
    {
        public CommodoreDBContext():base("CommodoreDB")
        {
            Database.SetInitializer(new CommodoreDBInitializer());
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<GameReview> GameReviews { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
