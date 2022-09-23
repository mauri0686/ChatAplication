using System.ComponentModel.DataAnnotations;

namespace ChatBackend.Models;

public class Message
{
    [Key]
    public int id { get; set; }
    public Guid roomId { get; set; }
    public string userId { get; set; }
    public string? message { get; set; }
    public DateTime createdAt { get; set; }

    public User user { get; set; }
    public Room room { get; set; }
}