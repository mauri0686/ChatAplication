using Microsoft.AspNetCore.Identity;

namespace ChatBackend.Models
{
    public class User : IdentityUser
    {
        public ICollection<Room> rooms { get; set; }
        public ICollection<Message> messages { get; set; }
    }
}