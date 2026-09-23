using Microsoft.EntityFrameworkCore;

namespace GodSimulator.Data
{
    public class MyApplicationContext : DbContext
    {
        public DbSet<Update> Updates { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=moderation.db");
            }
        }
    }
}
