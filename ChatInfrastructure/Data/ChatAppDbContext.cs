
using ChatDomain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatInfrastruncture.Data
{
    public class  ChatAppDbContext : IdentityDbContext
    {
        public ChatAppDbContext(DbContextOptions<ChatAppDbContext> options)
            : base(options)
        {
            
        }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Message> Messages { get; set; }

    
    }
}