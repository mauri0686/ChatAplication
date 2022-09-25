
using System.Text.RegularExpressions;
using ChatDomain.Models;
using ChatInfrastruncture.Data;
using ChatInfrastruncture.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatBackend.Hub
{
    public class ChatHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly string _botUser;
        private readonly IDictionary<string, UserConnection> _connections;
        private readonly UserService _userService;
        private readonly MessageService _messageService;
        private readonly RoomService _roomService;
        private readonly StockService _stockService;
        private readonly UserManager<IdentityUser> _userManager;
        public ChatHub(IDictionary<string, UserConnection> connections,  UserManager<IdentityUser> userManager, UserService userService, MessageService messageService,RoomService roomService, StockService stockService )
        {
            _botUser = "Bot Moderator";            
            _connections = connections;
            _userService = userService;
            _messageService = messageService;
            _roomService = roomService;
            _userManager = userManager;
            _stockService = stockService;
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (_connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
            {
                _connections.Remove(Context.ConnectionId);
                Clients.Group(userConnection.Room.name).SendAsync("ReceiveMessage", _botUser, $"{userConnection.User.UserName} has left");
                SendUsersConnected(userConnection.Room.name);
            }

            return base.OnDisconnectedAsync(exception);
        }

        public async Task JoinRoom(UserConnection userConnection)
        {
            
            
            _connections[Context.ConnectionId] = userConnection;
           
            await Task.WhenAll(
                Task.Run(async () =>
                {
                    userConnection.User =  await _userManager.FindByEmailAsync(userConnection.UserEmail);
                    userConnection.Room= await _roomService.Get(Convert.ToInt16(userConnection.RoomId));
                    
                        
                })
                    .ContinueWith(async _ =>
                    {
                        await Groups.AddToGroupAsync(Context.ConnectionId,userConnection.Room.name );

                    })
                    .ContinueWith(async _ =>
                    {
                        await Clients.Group(userConnection.Room.name ).SendAsync("ReceiveMessage", _botUser,$"{userConnection.User?.UserName} has joined {userConnection.Room.name }");
                        await SendUsersConnected(userConnection.Room.name);
                    }));



        }

        public async Task SendMessage(string message)
        {
            if (_connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
            {
                
                await _messageService.Add(new Message(){ message=message, roomId = Convert.ToInt16(userConnection.RoomId), userId = userConnection.User.Id ,createdAt = DateTime.Now});
                await Clients.Group(userConnection.Room.name)
                    .SendAsync("ReceiveMessage", userConnection.User.UserName, message);
                await ProcessMessages(message);
            }
        }
        public async Task SendBotMessage(Message message)
        {
            if (_connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
            {

                await Clients.Group(userConnection.Room.name)
                    .SendAsync("ReceiveMessage", _botUser , message.message);
            }
        }
        /// <summary>
        /// Method in charge of look for a command
        /// </summary>
        /// <param name="message"></param>
        private async Task ProcessMessages(string message)
        {
            var regex = new Regex("/stock=(.*)");
            if (regex.IsMatch(message.ToLower()))
            {
                var stock = regex.Match(message.ToLower()).Groups[1].Value;
                var msg = await _stockService.GetStockData(stock);
                await SendBotMessage(msg);
            }

            }
      
        /// <summary>
        /// Sends Users connected to the queue
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public Task SendUsersConnected(string room)
        {
            var users = _connections.Values
                .Where(c => c.Room.name == room)
                .Select(c => c.User.UserName);

            return Clients.Group(room).SendAsync("UsersInRoom", users);
        }

      
    }
}
