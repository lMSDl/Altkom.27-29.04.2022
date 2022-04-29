using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Hubs
{
    public class UsersHub : Hub
    {

        public override async Task OnConnectedAsync()
        {
            Console.WriteLine(Context.ConnectionId);
            await Clients.Others.SendAsync($"New Client: {Context.ConnectionId}");

            await Clients.Client(Context.ConnectionId).SendAsync("Welcome", "Welcome in UsersHub");
            await Clients.Others.SendAsync("NewConnection", Context.ConnectionId);

            await base.OnConnectedAsync();
        }

        public async Task SendMessageTo(string connectionId, string message)
        {
            await Clients.Client(connectionId).SendAsync(nameof(SendMessageTo), Context.ConnectionId, message);
        }
        public async Task SendMessage(string message)
        {
            await Clients.Others.SendAsync(nameof(SendMessage), Context.ConnectionId, message);
        }

        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }


    }
}
