using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;

namespace RepositoryLayer.Service
{
    public class GreetingRL : IGreetingRL
    {
        private readonly GreetingContext _dbContext;
        public GreetingRL(GreetingContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void SaveGreeting(string message)
        {
            var greeting = new GreetingMessage { Message = message, CreatedAt = DateTime.UtcNow };
            _dbContext.Greetings.Add(greeting);
            _dbContext.SaveChanges();
        }
    }
}
