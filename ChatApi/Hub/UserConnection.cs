using ChatDomain.Models;
using Microsoft.AspNetCore.Identity;

namespace ChatBackend.Hub;

public class UserConnection
{
 
    public string? UserEmail { get; set; }
    public string? RoomId { get; set; }
    public Room Room { get; set; }
    
    public IdentityUser? User { get; set; }
}