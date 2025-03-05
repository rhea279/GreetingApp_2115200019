
using ModelLayer.Model;

namespace RepositoryLayer.Interface
{
    public interface IGreetingRL
    {
        GreetingMessage GetGreetingById(int id);
        void SaveGreeting(string message);
        List<GreetingMessage> GetAllGreetings();
    }
}
