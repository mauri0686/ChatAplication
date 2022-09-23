namespace ChatBackend.Models;

public class Room
{
    public Guid id { get; set; }
    public string name { get; set; }
    //public ICollection<User> users { get; set; }
    public ICollection<Message> messages { get; set; }
}
 
