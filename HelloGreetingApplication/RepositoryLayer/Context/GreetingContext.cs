using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity; 

namespace RepositoryLayer.Context
{
    public class GreetingContext : DbContext
    {
        public GreetingContext(DbContextOptions<GreetingContext> options) : base(options)
        {
        }

        public DbSet<Entry> Entries { get; set; } 
    }
}
