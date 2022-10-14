using ChatDomain.Models;
using ChatInfrastruncture;
using ChatInfrastruncture.Data;
using ChatInfrastruncture.Service;
using Microsoft.EntityFrameworkCore;
using Moq;


namespace ChatUnitTest;

public class MessageTest
{
    private DbContextOptions<ChatAppDbContext> dbContextOptions;
   
    
    private Mock<IRepository<Message>> _messageMock;
    private Mock<MessageService> _mockService;
    private MessageService _messageService;

    [SetUp]
    public void Setup()
    {
        _messageMock = new Mock<IRepository<Message>>();
        _mockService = new Mock<MessageService>(_messageMock.Object);
        _messageService = new MessageService(_messageMock.Object);
    }

 
    [Test]
    public async Task InsertMessage()
    {
        var messages = new List<Message>()
            {
               new() { message = "Hi" ,createdAt = DateTime.Now},
               new() { message = "Test", createdAt = DateTime.Now, userId = "1", roomId = 1},
               new() { message = "Hello" ,createdAt = DateTime.Now},
               new() { message = "Bye", createdAt = DateTime.Now, userId = "1", roomId = 1},
            };

        _messageMock.Setup(  p =>  p.GetAll()).ReturnsAsync(messages);
        // var result = await _messageService.GetRoomMessageLimit(1,1);
        //Assert.That(result.Count, Is.EqualTo(1));

    }
 
   
}