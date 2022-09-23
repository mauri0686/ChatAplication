
using ChatBackend.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatBackend.Data
{
    public class  ChatAppDbContext : IdentityDbContext
    {
        public ChatAppDbContext(DbContextOptions<ChatAppDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasMany(x => x.messages)
                .WithOne(x => x.user);

            builder.Entity<Room>()
                .HasMany(x => x.messages)
                .WithOne(x => x.room);
        }
    }
}