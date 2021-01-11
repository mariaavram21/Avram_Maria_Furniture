using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Avram_Maria_Furniture.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            //await Clients.All.SendAsync("ReceiveMessage", Context.User.Identity.Name, user, message);
            await Clients.All.SendAsync("ReceiveMessage", user, message, DateTime.Now.ToString("HH:mm:ss"));
        }
    }
}
