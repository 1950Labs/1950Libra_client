using LibraReactClient.BusinessLayer.Entities;
using Microsoft.EntityFrameworkCore;


namespace LibraReactClient.DataAccess
{
    public class LibraContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=libra.db");
            
        }
    }
    
}
