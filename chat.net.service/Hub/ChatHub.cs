using Microsoft.AspNetCore.SignalR;

namespace ChatService.Hub;

public class ChatHub :Microsoft.AspNetCore.SignalR.Hub
{
    private readonly string _bot;
    public ChatHub()
    {
        _bot = "Chat Bot";
    }
    public async Task JoinRoom(ChatApp chatApp)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatApp.Room);
        await Clients.Group(chatApp.Room).SendAsync("Message", "Hello World", $"{chatApp.User} joined {chatApp.Room}");
    }
}