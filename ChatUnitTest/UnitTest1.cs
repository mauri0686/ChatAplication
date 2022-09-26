using ChatDomain.Models;
using ChatInfrastruncture;
using ChatInfrastruncture.Data;
using ChatInfrastruncture.Service;
using Microsoft.EntityFrameworkCore;
using Moq;


namespace ChatUnitTest;

public class Tests
{
    private DbContextOptions<ChatAppDbContext> dbContextOptions;
   
    
    [SetUp]
    public void Setup()
    {
        var dbName = $"RoomDbTest_{DateTime.Now.ToFileTimeUtc()}";
        dbContextOptions = new DbContextOptionsBuilder<ChatAppDbContext>() 
            .UseInMemoryDatabase(dbName)
            .Options;
    }

    [Test]
    public async Task GetRoomSucces()
    {
        var repository = await CreateRepositoryAsync();

        // Act
        var roomList = await repository.GetAll();

        // Assert
        Assert.That(roomList.Count, Is.EqualTo(1));
    }
    private async Task<Repository<Room>> CreateRepositoryAsync()
    {
        ChatAppDbContext context = new ChatAppDbContext(dbContextOptions);
        await PopulateDataAsync(context);
        return  new Repository<Room>(context);
    }
    private async Task PopulateDataAsync(ChatAppDbContext context)
    {
       
            var room = new Room()
            {
                id=1,
                name = "Room1",
                
            };
            await context.Rooms.AddAsync(room);
        

        await context.SaveChangesAsync();
    }
}