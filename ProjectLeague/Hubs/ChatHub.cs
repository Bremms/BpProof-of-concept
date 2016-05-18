using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ProjectLeague.Hubs
{
    public class ChatHub : Hub
    {
        public Task SendMessage(string content, string grpName)
        {
            return Clients.Group(grpName).RetrieveMessage(content);
        }
    }
}