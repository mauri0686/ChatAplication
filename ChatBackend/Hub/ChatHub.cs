using ChatBackend.Data;
using ChatBackend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatBackend.Hub
{
    public class ChatHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly string _botUser;
        private readonly IDictionary<string, UserConnection> _connections;
        private static ChatAppDbContext _context;
        public ChatHub(IDictionary<string, UserConnection> connections, ChatAppDbContext context)
        {
            _botUser = "Bot Moderator";            
            _connections = connections;
            _context = context;
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (_connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
            {
                _connections.Remove(Context.ConnectionId);
                Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", _botUser, $"{userConnection.UserName} has left");
                SendUsersConnected(userConnection.Room);
            }

            return base.OnDisconnectedAsync(exception);
        }

        public async Task JoinRoom(UserConnection userConnection)
        {
            
            IdentityUser? user = null;
            Room? room = null;
            _connections[Context.ConnectionId] = userConnection;
           
            await Task.WhenAll(
                Task.Run(async () =>
                {
                    user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userConnection.UserEmail);
                    room= await _context.Rooms.FirstOrDefaultAsync(x => x.id.ToString() ==userConnection.RoomId);
                        
                })
                    .ContinueWith(async _ =>
                    {
                        await Groups.AddToGroupAsync(Context.ConnectionId,room.name );
                        
                    })
                    .ContinueWith(async _ =>
                    {
                        userConnection.Room = room.name;
                        userConnection.UserName = user.UserName;
                        await Clients.Group(userConnection.Room ).SendAsync("ReceiveMessage", _botUser,$"{user?.UserName} has joined {userConnection.Room }");
                        await SendUsersConnected(userConnection.Room );
                    }));



        }

        public async Task SendMessage(string message)
        {
            if (_connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
            {
                
                await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", userConnection.UserName, message);
            }
        }

        public Task SendUsersConnected(string room)
        {
            var users = _connections.Values
                .Where(c => c.Room == room)
                .Select(c => c.UserName);

            return Clients.Group(room).SendAsync("UsersInRoom", users);
        }
    }
}
