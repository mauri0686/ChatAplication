using System.ComponentModel.DataAnnotations;

namespace ChatBackend.Models;

public class Message
{
    [Key]
    public int id { get; set; }
    public int roomId { get; set; }
    public string userId { get; set; }
    public string? message { get; set; }
    public DateTime createdAt { get; set; }

}