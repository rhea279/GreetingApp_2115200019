using Microsoft.EntityFrameworkCore;
using ModelLayer.Model;
using RepositoryLayer.Entity; 

namespace RepositoryLayer.Context
{
    public class GreetingContext : DbContext
    {
        public GreetingContext(DbContextOptions<GreetingContext> options) : base(options)
        {
        }
        public DbSet<GreetingMessage> Greetings { get; set; }
        public DbSet<Entry> Entries { get; set; } 
    }
}
