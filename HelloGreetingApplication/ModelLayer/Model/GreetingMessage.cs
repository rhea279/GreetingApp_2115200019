using System.ComponentModel.DataAnnotations;

using System.Threading.Tasks;

namespace ModelLayer.Model
{
    public class GreetingMessage
    {
        [Key]
        public int Id { get; set; }

        public string Message { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
