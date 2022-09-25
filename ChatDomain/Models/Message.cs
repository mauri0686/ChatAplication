using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ChatDomain.Models;

public class Message: BaseEntity
{
    
    public int roomId { get; set; }

    public string? message { get; set; }
    public DateTime createdAt { get; set; }
    
    [ForeignKey("userId")]
    public IdentityUser? user { get; set; }

    [Required]
    public string userId { get; set; }


}