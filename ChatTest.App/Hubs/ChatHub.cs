using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChatTest.App.Models;
using ChatTest.App.Services;
using Microsoft.AspNetCore.SignalR;

namespace ChatTest.App.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IUserService _userService;
        public const string SendMessageMethod = "messageReceived";
        public const string UserConnectedMethod = "userConnected";
        public const string UserDisconnectedMethod = "userDisconnected";



        public ChatHub(IUserService userService)
        {
            _userService = userService;
        }



        public async Task UserConnected()
        {
            var user = _userService.GetUserByConnection(Context.ConnectionId);

            if (user == null)
                return;

            await Clients.All.SendAsync(UserConnectedMethod, user.Name);
        }



        public override async Task OnConnectedAsync()
        {
            try
            {
                var user = _userService.GetUserByConnection(Context.ConnectionId);

                if (user == null)
                    return;

                user.Online = true;

                await Clients.All.SendAsync(UserConnectedMethod, user.Name);
            }
            finally
            {
                await base.OnConnectedAsync();
            }
        }



        public override async Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                var user = _userService.GetUserByConnection(Context.ConnectionId);

                if (user == null)
                    return;

                user.Online = false;

                await Clients.All.SendAsync(UserDisconnectedMethod, user.Name);
            }
            finally
            {
                await base.OnDisconnectedAsync(exception);
            }
        }
    }
}
