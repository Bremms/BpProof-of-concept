using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace ProjectLeague.Hubs
{
    [Authorize]
    public class TestHub : Hub
    {
        public override Task OnConnected()
        {
            return Clients.All.hello("welcome");
        }
        public Task Hello(string s)
        {
            while(true)
            {
                System.Threading.Thread.Sleep(100);
                Clients.All.hello("eej fijnen");
            }
           
        }
    }
}